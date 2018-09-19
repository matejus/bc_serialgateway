using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace Adastra.BigClownGateway
{
    public class BcSerialPort:IDisposable
    {
        SerialPort _port;

        /// <summary>
        /// Serial port name e.g. COM3
        /// </summary>
        public string Com { get; private set; }

        /// <summary>
        /// type of BigClown device, see <see cref="BcDeviceType"/>
        /// </summary>
        public BcDeviceType DeviceType { get; private set; }

        private BcSerialPortDetail _detail;
        public BcSerialPortDetail Detail
        {
            get
            {
                if (_detail == null)
                    _detail = new BcSerialPortDetail(this);
                return _detail;
            }
        }

        /// <summary>
        /// Raised when MQTT message from <see cref="BcDeviceType"/> is received
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Raised when received data are not MQTT message
        /// </summary>
        public event EventHandler<DataEventArgs> RawDataReceived;

        /// <summary>
        /// Raised when <see cref="BcDeviceType"/> is removed from the computer
        /// </summary>
        public event EventHandler<EventArgs> ConnectionChanged;


        private BcSerialPort(string com, BcDeviceType type)
        {
            Com = com;
            DeviceType = type;
        }

        private BcInfo _info;

        public BcInfo Info
        {
            get
            {
                if (_info == null)
                {
                    MqttMessage msg = null;
                    if (ExecuteWithOpen(() => { msg = WaitForResponse(BcCommands.Info, "/info"); }))
                    {
                        if (msg != null)
                            _info = BcInfo.CreateFromMessage(msg);
                    }
                }
                return _info;
            }
        }

        private BcNodeCollection _nodes = new BcNodeCollection();
        public BcNode[] Nodes
        {
            get
            {
                var msg = WaitForResponse(BcCommands.GetNodes, "/nodes");
                if (msg != null && !string.IsNullOrEmpty(msg.Payload))
                    _nodes.SyncFromList(msg.Payload);

                return _nodes.ToArray();
            }
        }

        #region open / close

        /// <summary>
        /// create new underlaying <see cref="SerialPort"/> and open it for communication
        /// </summary>
        public void Open()
        {
            if (_port != null && _port.IsOpen)
                throw new CommunicationException("Port is already open");

            if (_port == null)
            {
                _port = new SerialPort(Com, DeviceType.BaudRate, DeviceType.Parity, DeviceType.DataBits, DeviceType.StopBits);
                _port.DataReceived += SerialPort_DataReceived;
                _port.ErrorReceived += SerialPort_ErrorReceived;
            }

            try
            {
                _port.Open();
            }
            catch ( Exception error)
            {
                throw new CommunicationException(error.Message, error);
            }
        }

        /// <summary>
        /// gets the information the underlaying <see cref="SerialPort"/> is open for communication
        /// </summary>
        public bool IsOpen => _port != null ? _port.IsOpen : false;


        /// <summary>
        ///  Close internal <see cref="SerialPort"/>
        /// </summary>
        public void Close()
        {
            if (_port != null)
            {
                if (_port.IsOpen)
                    _port.Close();
                _port.DataReceived -= SerialPort_DataReceived;
                _port.Dispose();
                _port = null;
            }
        }
        public void Dispose()
        {
            Close();
        }

        #endregion

        public void SendMessage(MqttMessage msg)
        {
            if (_port == null && !_port.IsOpen)
                throw new CommunicationException("Port is not open for communication");

            _port.WriteLine(msg.ToMessageString());
        }

        #region open if required

        /// <summary>
        /// try to open post if is closed and execute request/response message
        /// </summary>
        /// <param name="action"></param>
        /// <returns>false when port cannot be open</returns>
        private bool ExecuteWithOpen(Action action)
        {
            bool ok = true;
            bool closePort = false;
            try
            {
                if (_port == null || _port.IsOpen)
                {
                    Open();
                    closePort = true;
                }

                action();
            }
            catch
            {
                ok = false;
            }
            finally
            {
                if (closePort && _port.IsOpen)
                    _port.Close();
            }

            return ok;
        }

        #endregion

        #region sync call over the serial port

        /// <summary>
        /// list of requested topics
        /// </summary>
        private List<AwaitingTask> _waitList = new List<AwaitingTask>();

        /// <summary>
        /// send message and wait for received response
        /// </summary>
        /// <param name="toSend"></param>
        /// <param name="expectedTopic">start of the response topic</param>
        /// <returns></returns>
        private MqttMessage WaitForResponse(MqttMessage toSend, string expectedTopic, int timeout = Timeout.Infinite)
        {
            var task = RegisterWait(expectedTopic, timeout);
            SendMessage(toSend);
            task.WaitOne();

            if (task.TimeoutExpired)
                RemoveFromWait(task);

            return task.Response;
        }

        private AwaitingTask RegisterWait(string expectedResponse, int timeout)
        {
            AwaitingTask task = new AwaitingTask(expectedResponse, timeout);
            lock (_waitList)
            {
                _waitList.Add(task);
            }

            return task;
        }

        private bool ReleaseWait(MqttMessage receivedMessage)
        {
            if (receivedMessage == null)
                return false;

            lock (_waitList)
            {
                var task = _waitList.FirstOrDefault(t => receivedMessage.Topic.StartsWith(t.ExpectedResponse));
                if (task != null)
                {
                    _waitList.Remove(task);
                    task.Response = receivedMessage;
                    task.Set();
                    return true;
                }
            }

            return false;
        }

        private void RemoveFromWait(AwaitingTask task)
        {
            lock (_waitList)
            {
                if (_waitList.Contains(task))
                    _waitList.Remove(task);
            }
        }

        #endregion sync call

        #region receive data from serial port

        /// <summary>
        /// part of the message sent in <see cref="SerialPort.DataReceived"/> event which does not finish by EOL
        /// </summary>
        private string _restOfLine = string.Empty;

        /// <summary>
        /// process received data, convert it to the message and raise events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (MessageReceived == null && RawDataReceived == null && _waitList.Count == 0)
                return;

            if (e.EventType == SerialData.Eof)
                return;

            string allData = _port.ReadExisting();
            if (string.IsNullOrEmpty(allData))
                return;

            // combine with previous uncomplete line
            lock (_restOfLine)
            {
                allData = _restOfLine + allData;
                _restOfLine = string.Empty;

                int eol = allData.LastIndexOf('\n');
                if (eol < 0)
                {
                    _restOfLine = allData;
                    return;
                }
                else if (eol < allData.Length)
                {
                    _restOfLine = allData.Substring(eol + 1);
                    allData = allData.Substring(0, eol);
                }
            }

            foreach (var data in allData.Split(new char[] { '\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                ProcessReceivedData(data.Replace("\r", string.Empty));
            }
        }

        /// <summary>
        /// raises event depends on the received data and format
        /// </summary>
        /// <param name="data"></param>
        private void ProcessReceivedData(string data)
        {
            MqttMessage msg;
            if (MqttMessage.TryParse(data, out msg))
            {
                // nothing on waitlist or response is not expected by waiting tasks
                if (_waitList.Count == 0 || !ReleaseWait(msg))
                {
                    CollectPortInformation(msg);
                    MessageReceived?.Invoke(this, new MessageEventArgs(msg));
                }
            }
            else
            {
                RawDataReceived?.Invoke(this, new DataEventArgs(data));
            }
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //throw new CommunicationException("ERROR");
        }

        #endregion receive

        #region collect port information

        private void CollectPortInformation(MqttMessage msg)
        {
            if (msg == null || string.IsNullOrEmpty(msg.Topic))
                return;

            if (msg.Topic.StartsWith("/info"))
            {
                if (_info == null)
                    _info = BcInfo.CreateFromMessage(msg);
            }
            else
            {
                _nodes.SyncFromMessage(msg);
            }
        }

        #endregion

        public override string ToString()
        {
            return $"{Com} - {DeviceType.Type}";
        }

        #region static create methods

        public static async Task<BcSerialPort> CreateAsync(string portName)
        {
            BcSerialPort port = null;
            string selector = SerialDevice.GetDeviceSelector(portName);
            var task = DeviceInformation.FindAllAsync(selector);
            while (task.Status == Windows.Foundation.AsyncStatus.Started)
                await Task.Delay(20);
            var devices = task.GetResults();

            if (devices.Any(d => d.IsEnabled && (d.Id.Contains(BcDeviceType.Core.ID))))
                port = new BcSerialPort(portName, BcDeviceType.Core);
            else if (devices.Any(d => d.IsEnabled && d.Id.Contains(BcDeviceType.Dongle.ID)))
                port = new BcSerialPort(portName, BcDeviceType.Dongle);

            return port;
        }

        public static async Task<BcSerialPort[]> GetBcSerialPortsAsync()
        {
            string[] comPorts = SerialPort.GetPortNames();

            List<BcSerialPort> clowns = new List<BcSerialPort>();

            foreach(var com in comPorts)
            {
                BcSerialPort port = await CreateAsync(com);
                if (port != null)
                    clowns.Add(port);
            }
            
            return clowns.ToArray();
        }

        public static async Task<BcSerialPort> CreateByBcIdAsync(string bcId)
        {
            var allPorts = await GetBcSerialPortsAsync();
            BcSerialPort port = allPorts.FirstOrDefault(p => p.Info?.ID == bcId);
            return port;
        }

        #endregion static create

    }


}
