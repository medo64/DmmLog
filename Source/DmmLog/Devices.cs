using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace DmmLog {
    internal static class Devices {

        public static void Load() {
            var items = new List<Device>();

            for (int i = 0; i < Devices.MaxItems; i++) {
                var displayName = Medo.Configuration.Settings.Read(string.Format(CultureInfo.InvariantCulture, "Device[{0}].DisplayName", i), null);
                var driverName = Medo.Configuration.Settings.Read(string.Format(CultureInfo.InvariantCulture, "Device[{0}].Driver", i), null);
                var settings = Medo.Configuration.Settings.Read(string.Format(CultureInfo.InvariantCulture, "Device[{0}].Settings", i), "");
                if (!(string.IsNullOrEmpty(driverName))) {
                    var driver = Drivers.GetDriver(driverName);
                    if (driver == null) {
                        Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "W: Cannot find driver '{1}' for device '{0}'.", displayName, driverName));
                    } else {
                        try {
                            var device = new Device(displayName, driver, settings);
                            if (device != null) {
                                Debug.WriteLine("I: Loaded device '" + device.ToString() + "' using driver '" + driverName + "'.");
                                items.Add(device);
                            }
                        } catch (TargetInvocationException ex) {
                            Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "W: Cannot load device '{0}' using driver '{1}'; {2}.", displayName, driverName, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message));
                        }
                    }
                }
            }


            Devices.Items = items;
        }

        public static void Save() {
            for (int i = 0; i < Devices.Items.Count; i++) {
                var item = Devices.Items[i];
                Medo.Configuration.Settings.Write(string.Format(CultureInfo.InvariantCulture, "Device[{0}].DisplayName", i), item.DisplayName);
                Medo.Configuration.Settings.Write(string.Format(CultureInfo.InvariantCulture, "Device[{0}].Driver", i), item.Driver.Name);
                Medo.Configuration.Settings.Write(string.Format(CultureInfo.InvariantCulture, "Device[{0}].Settings", i), item.Settings);
            }
            for (int i = Devices.Items.Count; i < Devices.MaxItems; i++) {
                Medo.Configuration.Settings.Write(string.Format(CultureInfo.InvariantCulture, "Device[{0}].DisplayName", i), null);
                Medo.Configuration.Settings.Write(string.Format(CultureInfo.InvariantCulture, "Device[{0}].Driver", i), null);
                Medo.Configuration.Settings.Write(string.Format(CultureInfo.InvariantCulture, "Device[{0}].Settings", i), null);
            }
        }


        public static Device Add(Driver driver, String displayName, String settings) {
            if (Devices.Items.Count >= Devices.MaxItems) { throw new InvalidOperationException("Cannot have more than 16 devices."); }
            var device = new Device(displayName, driver, settings);
            Devices.Items.Add(device);
            return device;
        }

        public static bool Remove(Device device) {
            return Devices.Items.Remove(device);
        }


        private static List<Device> Items = new List<Device>();
        private static readonly double MaxItems = 16;


        public static IEnumerable<Device> LoadedDevices {
            get { return Devices.Items.AsReadOnly(); }
        }

    }
}
