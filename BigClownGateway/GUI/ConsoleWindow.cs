using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adastra.BigClownGateway.GUI
{
    public partial class ConsoleWindow : Form
    {
        public ConsoleWindow()
        {
            InitializeComponent();
            AppendLogDelegate = new AppendLog(AppendWatchLog);

            this.Load += ConsoleWindow_Load;
        }

        private async void ConsoleWindow_Load(object sender, EventArgs e)
        {
            var coms = await BcSerialPort.GetBcSerialPortsAsync();
            foreach (var c in coms)
            {
                cbCom.Items.Add(c);
            }
            if (coms.Length > 0)
                cbCom.SelectedIndex = 0;

            tbMessage.Items.Add(BcCommands.Info.Topic);
            tbMessage.Items.Add(BcCommands.PairingModeStart.Topic);
            tbMessage.Items.Add(BcCommands.PairingModeStop.Topic);
            tbMessage.Items.Add(BcCommands.GetNodeInfo.Topic);
            tbMessage.Items.Add(BcCommands.GetNodes.Topic);
        }

        UsbWatcher _watcher;
        private void btnWatcher_Click(object sender, EventArgs e)
        {
            if (_watcher == null)
            {
                _watcher = UsbWatcher.Create();
                _watcher.DeviceChanged += _watcher_DeviceChanged;
                _watcher.Start();
            }
            else
            {
                _watcher.DeviceChanged -= _watcher_DeviceChanged;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        private void _watcher_DeviceChanged(object sender, UsbWatcherEventArgs e)
        {
            AppendWatchLog(tbWatcherLog, e.ToString() + "\r\n");
        }

        delegate void AppendLog(TextBox tb, string message);
        private AppendLog AppendLogDelegate;
        private void AppendWatchLog(TextBox tb, string message)
        {
            if (tb.InvokeRequired)
            {
                this.Invoke(AppendLogDelegate, tb, message);
            }
            else
            {
                tb.Text += message;
                tb.Select(tb.Text.Length, 0);
                tb.ScrollToCaret();
            }
        }

        List<BcSerialPort> _ports = new List<BcSerialPort>();
        private void btnOpen_Click(object sender, EventArgs e)
        {
            BcSerialPort port = cbCom.SelectedItem as BcSerialPort;
            if (port != null)
            {
                if (!port.IsOpen)
                    OpenPort(port);
                else
                    ClosePort(port);
            }

            CheckPortStatus();

            SetButtonText();
        }

        void OpenPort(BcSerialPort port)
        {
            try
            {
                port.MessageReceived += _port_MessageReceived;
                port.RawDataReceived += _port_RawDataReceived;
                port.Open();

            }
            catch (Exception error)
            {
                port.MessageReceived -= _port_MessageReceived;
                port.RawDataReceived -= _port_RawDataReceived;

                MessageBox.Show(error.Message);
            }

        }
        void ClosePort(BcSerialPort port)
        {
            try
            {
                port.Close();
                port.MessageReceived -= _port_MessageReceived;
                port.RawDataReceived -= _port_RawDataReceived;
            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }

        }


        private void SetButtonText()
        {
            BcSerialPort port = cbCom.SelectedItem as BcSerialPort;
            if (port == null)
            {
                btnOpen.Enabled = false;
                btnOpen.Text = "No device";
                return;
            }

            btnOpen.Text = port.IsOpen ? "Close" : "Open";
            btnOpen.Enabled = true;
        }

        private void _port_RawDataReceived(object sender, DataEventArgs e)
        {
            string txt = $"***> {e.Value}\r\n";
            AppendWatchLog(tbResponse, txt);
        }

        private void _port_MessageReceived(object sender, MessageEventArgs e)
        {
            string txt = e.Message.ToMessageString() + "\r\n";
            AppendWatchLog(tbResponse, txt);
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            MqttMessage msg;
            if (!MqttMessage.TryParse(tbMessage.Text, out msg))
                msg = new MqttMessage { Topic = tbMessage.Text };

            string txt = $"---> {msg.ToMessageString()}\r\n";
            AppendWatchLog(tbResponse, txt);

            BcSerialPort port = cbCom.SelectedItem as BcSerialPort;
            if (port!=null)
                port.SendMessage(msg);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            BcSerialPort port = cbCom.SelectedItem as BcSerialPort;
            if (port == null)
            {
                MessageBox.Show("No opened serial port");
                return;
            }

            var info = port.Info;
            AppendWatchLog(tbResponse, $"GW>{info.ID} FIRMWARE>{info.Firmware}\r\n");

            var nodes = port.Nodes;

            if (nodes.Length == 0)
            {
                AppendWatchLog(tbResponse, "NODE > NOTHING FOUND !!!\r\n");
            }
            else
            {
                foreach (var n in nodes)
                {
                    string txt = $"NODE > {n.Info.ID}\r\n";
                    foreach (var s in n.Sensors)
                        txt += $"\t{s}\r\n";
                    AppendWatchLog(tbResponse, txt);
                }
            }


        }

        private void cbCom_SelectedValueChanged(object sender, EventArgs e)
        {
            CheckPortStatus();
            SetButtonText();
        }

        private void CheckPortStatus()
        {
            var com = cbCom.SelectedItem as BcSerialPort;
            if (com == null)
            {
                cbxEnabled.CheckState = cbxOccupied.CheckState = cbxOpen.CheckState = CheckState.Indeterminate;
                lblDeviceID.Text = lblBcId.Text = string.Empty;
                return;
            }

            cbxEnabled.Checked = com.Detail.IsEnabled;
            cbxOpen.Checked = com.IsOpen;
            cbxOccupied.Checked = com.Detail.IsOccupied;
            lblDeviceID.Text = com.Detail.DeviceID;
            lblBcId.Text = com.Info?.ID;

            panMsg.Enabled = com.IsOpen;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMessage.Text = "";
            tbResponse.Text = "";
        }
    }
}
