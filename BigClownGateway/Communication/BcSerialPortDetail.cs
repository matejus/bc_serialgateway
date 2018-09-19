using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace Adastra.BigClownGateway
{
    public class BcSerialPortDetail
    {
        DeviceInformation _info;

        BcSerialPort _parent;

        public bool IsEnabled => (_info?.IsEnabled) ?? false;

        public bool IsOccupied
        {
            get
            {
                if (_parent.IsOpen)
                    return false;

                // try to open to identify the port is occupied by another application
                SerialPort p = new SerialPort(_parent.Com);
                try
                {
                    if (!p.IsOpen)
                        p.Open();
                }
                catch
                {
                    return true;
                }
                finally
                {
                    p.Close();
                }
                return false;

            }
        }

        public string DeviceID { get; private set; }

        public string FullID => _info?.Id;

        public string Name => _info?.Name;

        public EnclosureLocation Location => _info?.EnclosureLocation;

        public DeviceInformationKind Kind => (_info?.Kind) ?? DeviceInformationKind.Unknown;

        public Guid InstanceGuid
        {
            get
            {
                int guidLen = Guid.NewGuid().ToString().Length + 2;  //brackets
                if (_info == null || string.IsNullOrEmpty(_info.Id) || _info.Id.Length < 16 + guidLen)
                    return Guid.Empty;

                var value = _info.Id.Substring(_info.Id.Length - guidLen);
                Guid guid = Guid.Empty;
                Guid.TryParse(value, out guid);

                return guid;
            }
        }


        public BcSerialPortDetail(BcSerialPort parent)
        {
            System.Diagnostics.Debug.Assert(parent != null);

            _parent = parent;

            var aqs = SerialDevice.GetDeviceSelector(_parent.Com);
            var task = DeviceInformation.FindAllAsync(aqs);
            while (task.Status != Windows.Foundation.AsyncStatus.Completed)
            {
                Task.Delay(1);
            }
            var devices = task.GetResults();

            var di = devices.FirstOrDefault(d => d.IsEnabled);
            if (di is null)
                di = devices.FirstOrDefault();
            // no device?
            _info = di;

            if (di?.Properties != null)
            {
                const string PROP_DEVICEID = "System.Devices.DeviceInstanceId";
                if (di.Properties.ContainsKey(PROP_DEVICEID))
                    DeviceID = di.Properties[PROP_DEVICEID] as string;
            }
            
            //if (BcDeviceType.Dongle.IsTypeByID(_info?.Id))
            //{
            //    // check device using FTDI wrapper
            //    FTDI ftdi = new FTDI();
            //    uint deviceCount = 0;
            //    FTDI.FT_STATUS status = ftdi.GetNumberOfDevices(ref deviceCount); // Get the number of FTDI devices connected to the computer.
            //    if (status == FTDI.FT_STATUS.FT_OK && deviceCount>0)
            //    {
            //        FTDI.FT_DEVICE_INFO_NODE[] deviceList = new FTDI.FT_DEVICE_INFO_NODE[deviceCount]; // Allocate a storage array for a device info list.
            //        status = ftdi.GetDeviceList(deviceList);
            //        var port = deviceList.FirstOrDefault();

            //        string c;
            //        ftdi.GetCOMPort(out c);
            //        ftdi.OpenByLocation(port.LocId);
            //        ftdi.GetCOMPort(out c);
            //    }
            //}

        }
    }
}
