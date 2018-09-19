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
    public class BcDeviceType
    {
        public string Type { get; private set; }

        public ushort VID { get; private set; }
        public ushort PID { get; private set; }

        public string ID => $"VID_{VID:X4}+PID_{PID:X4}";

        public string DeviceSelector => SerialDevice.GetDeviceSelectorFromUsbVidPid(VID, PID);

        #region serial port configuration

        internal virtual int BaudRate => 115200;

        internal virtual Parity Parity => Parity.None;

        internal virtual int DataBits => 8;
        internal virtual StopBits StopBits => StopBits.One;

        #endregion

        /// <summary>
        /// usb device id from <see cref="DeviceInformation"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsTypeByID(string id)
        {
            return !string.IsNullOrEmpty(id) && id.Contains(ID);
        }

        public static BcDeviceType Dongle { get; } = new BcDeviceType { Type = "DONGLE", VID = 0x0403, PID = 0x6015 };
        public static BcDeviceType Core { get; } = new BcDeviceType { Type = "DONGLE", VID = 0x0438, PID = 0x5740 };
    }
}
