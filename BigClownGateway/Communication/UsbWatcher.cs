using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace Adastra.BigClownGateway
{
    class UsbWatcher : IDisposable
    {
        DeviceWatcher _watch;

        /// <summary>
        /// when device is not specified all usb devices will be watched 
        /// during the start only active devices are sent to WatchMessage event
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static UsbWatcher Create(BcDeviceType device = null)
        {
            DeviceWatcher watch = device!=null ? DeviceInformation.CreateWatcher(device.DeviceSelector) : DeviceInformation.CreateWatcher();

            return new UsbWatcher(watch);
        }

        private UsbWatcher(DeviceWatcher watch)
        {
            _watch = watch;

            _watch.Added += _watch_Added;
            _watch.Removed += _watch_Removed;
            _watch.Updated += _watch_Updated;
            _watch.EnumerationCompleted += _watch_EnumerationCompleted;
        }

        public void Dispose()
        {
            if (_watch != null && _watch.Status != DeviceWatcherStatus.Stopped && _watch.Status != DeviceWatcherStatus.Aborted)
                _watch.Stop();
        }

        public void Start()
        {
            _watch.Start();
        }


        bool _complete = false;
        private void _watch_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            _complete = true;
        }


        private void _watch_Added(DeviceWatcher sender, DeviceInformation args)
        {
            if (!args.IsEnabled)
                return;

            if (!_complete)
            {
                var t = args.EnclosureLocation;

                if (!args.Id.Contains(BcDeviceType.Core.ID) && !args.Id.Contains(BcDeviceType.Dongle.ID))
                    return;
            }

            DeviceChanged?.Invoke(this, new UsbWatcherEventArgs { Operation = UsbWatcherOperation.Added, Id = args.Id, Kind = args.Kind, IsEnabled=args.IsEnabled});
        }
        private void _watch_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            DeviceChanged?.Invoke(this, new UsbWatcherEventArgs { Operation = UsbWatcherOperation.Updated, Id = args.Id, Kind = args.Kind, IsEnabled = true });
        }

        private void _watch_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            DeviceChanged?.Invoke(this, new UsbWatcherEventArgs { Operation = UsbWatcherOperation.Removed, Id = args.Id, Kind = args.Kind, IsEnabled = false });
        }

        public event EventHandler<UsbWatcherEventArgs> DeviceChanged;
    }

    public enum UsbWatcherOperation
    {
        Added,
        Updated,
        Removed
    }

    public class UsbWatcherEventArgs : EventArgs
    {
        public UsbWatcherOperation Operation;
        public string Id;
        public DeviceInformationKind Kind;
        public bool IsEnabled;

        public override string ToString()
        {
            return $"{Operation}: {Kind} ==> ID: {Id} ==> Enabled={IsEnabled}";
        }
    }

}
