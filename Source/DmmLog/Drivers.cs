using DmmLogDriver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DmmLog {
    internal static class Drivers {

        private static Dictionary<String, Driver> DriversByName;

        public static void Initialize() {
            var directory = new DirectoryInfo(".");

            var drivers = new Dictionary<String, Driver>();

            Debug.WriteLine("I: Searching for drivers in \"" + directory + "\".");
            foreach (var file in directory.GetFiles("*.dll")) {
                var assembly = Assembly.LoadFile(file.FullName);
                foreach (var type in assembly.GetTypes()) {
                    if (type.IsPublic && (type.IsAbstract == false) && type.IsSubclassOf(typeof(DmmDriver))) {
                        var driver = new Driver(type);
                        drivers.Add(type.Name, driver);
                        Debug.WriteLine("I: Found '" + driver.Capabilities.DisplayName + "' in '" + file.Name + "'.");
                    }
                }
            }

            Drivers.DriversByName = drivers;
        }


        public static Driver GetDriver(string driverName) {
            Driver driver;
            if (Drivers.DriversByName.TryGetValue(driverName, out driver)) {
                return driver;
            } else {
                return null;
            }
        }

        public static IEnumerable<Driver> LoadedDrivers {
            get {
                var drivers = new List<Driver>();
                foreach (var item in Drivers.DriversByName) {
                    drivers.Add(item.Value);
                }

                drivers.Sort(
                    delegate(Driver driver1, Driver driver2) {
                        var manufacturer = string.Compare(driver1.Capabilities.Manufacturer, driver2.Capabilities.Manufacturer);
                        if (manufacturer != 0) {
                            if (driver1.Capabilities.Manufacturer == null) { return 1; }
                            if (driver2.Capabilities.Manufacturer == null) { return -1; }
                            return manufacturer;
                        } else {
                            return string.Compare(driver1.Capabilities.Model, driver2.Capabilities.Model);
                        }
                    });

                return drivers.AsReadOnly();
            }
        }

    }
}
