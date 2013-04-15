using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DmmLogDriver {
    /// <summary>
    /// Driver information.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public class DmmDriverInformation {

        internal DmmDriverInformation(Type type) {
            if (type == null) { throw new ArgumentNullException("type", "Type cannot be null."); }
            if (!type.IsSubclassOf(typeof(DmmDriver))) { throw new ArgumentOutOfRangeException("type", "Type is not a driver."); }

            this.Manufacturer = GetManufacturer(type);
            this.Model = GetModel(type);
            this.DisplayName = string.IsNullOrEmpty(this.Manufacturer) ? this.Model : this.Manufacturer + " " + this.Model;
            this.Interface = GetInterface(type);
        }

        /// <summary>
        /// Gets device manufacturer.
        /// </summary>
        public String Manufacturer { get; private set; }

        /// <summary>
        /// Gets device model.
        /// </summary>
        public String Model { get; private set; }

        /// <summary>
        /// Gets device manufacturer and model.
        /// </summary>
        public String DisplayName { get; private set; }

        /// <summary>
        /// Gets device interface.
        /// </summary>
        public DmmDriverInterface Interface { get; private set; }


        /// <summary>
        /// Returns general driver information.
        /// </summary>
        /// <param name="type">Type.</param>
        public static DmmDriverInformation GetDriverInformation(Type type) {
            if ((type != null) && type.IsSubclassOf(typeof(DmmDriver))) {
                return new DmmDriverInformation(type);
            } else {
                return null;
            }
        }

        #region Helpers

        private static String GetManufacturer(Type type) {
            if ((type != null) && type.IsSubclassOf(typeof(DmmDriver))) {
                var categoryAttr = type.GetCustomAttributes(typeof(CategoryAttribute), true);
                if (categoryAttr.Length > 0) {
                    return ((CategoryAttribute)categoryAttr[0]).Category.Trim();
                }
            }
            return null;
        }

        private static String GetModel(Type type) {
            if ((type != null) && type.IsSubclassOf(typeof(DmmDriver))) {
                var displayNameAttr = type.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (displayNameAttr.Length > 0) {
                    return ((DisplayNameAttribute)displayNameAttr[0]).DisplayName.Trim();
                }
                return type.Name;
            } else {
                return null;
            }
        }

        private static DmmDriverInterface GetInterface(Type type) {
            if ((type != null) && type.IsSubclassOf(typeof(DmmDriver))) {
                var descriptionAttr = type.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (descriptionAttr.Length > 0) {
                    var description = ((DescriptionAttribute)descriptionAttr[0]).Description.Trim().ToUpperInvariant();
                    switch (description) {
                        case "SERIAL": return DmmDriverInterface.SerialPort;
                    }
                }
            }
            return DmmDriverInterface.None;
        }

        #endregion

    }
}
