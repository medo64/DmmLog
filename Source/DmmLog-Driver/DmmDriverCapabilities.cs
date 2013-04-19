using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace DmmLogDriver {
    /// <summary>
    /// Driver information.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public class DmmDriverCapabilities {

        internal DmmDriverCapabilities(Type type) {
            if (type == null) { throw new ArgumentNullException("type", "Type cannot be null."); }
            if (!type.IsSubclassOf(typeof(DmmDriver))) { throw new ArgumentOutOfRangeException("type", "Type is not a driver."); }

            this.Manufacturer = DmmDriverCapabilities.GetPropertyValue(type, "Manufacturer");
            this.Model = DmmDriverCapabilities.GetPropertyValue(type, "Model");
            this.DisplayName = string.IsNullOrEmpty(this.Manufacturer) ? this.Model : this.Manufacturer + " " + this.Model;
            try {
                this.Interface = (DmmDriverInterface)Enum.Parse(typeof(DmmDriverInterface), DmmDriverCapabilities.GetPropertyValue(type, "Interface"));
            } catch (ArgumentException) {
                this.Interface = DmmDriverInterface.None;
            }

            int updateInterval;
            if (int.TryParse(DmmDriverCapabilities.GetPropertyValue(type, "UpdateInterval"), System.Globalization.NumberStyles.Integer, CultureInfo.InvariantCulture, out updateInterval)) {
                this.UpdateInterval = updateInterval;
            } else {
                this.UpdateInterval = 1000; //once per second is default
            }
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
        /// Gets device's update interval in milliseconds.
        /// </summary>
        public Int32 UpdateInterval { get; private set; }


        /// <summary>
        /// Returns general driver information.
        /// </summary>
        /// <param name="type">Type.</param>
        public static DmmDriverCapabilities GetDriverCapabilities(Type type) {
            if ((type != null) && type.IsSubclassOf(typeof(DmmDriver))) {
                return new DmmDriverCapabilities(type);
            } else {
                return null;
            }
        }


        #region Helpers

        private static Dictionary<Type, Dictionary<String, String>> PropertiesPerType = new Dictionary<Type, Dictionary<String, String>>();

        private static Dictionary<String, String> GetProperties(Type type) {
            Dictionary<string, string> properties;
            if (DmmDriverCapabilities.PropertiesPerType.TryGetValue(type, out properties)) {
                return properties;
            } else if ((type != null) && type.IsSubclassOf(typeof(DmmDriver))) { //parse properties
                properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                var categoryAttr = type.GetCustomAttributes(typeof(CategoryAttribute), true);
                if (categoryAttr.Length > 0) {
                    properties.Add("Manufacturer", ((CategoryAttribute)categoryAttr[0]).Category.Trim());
                }

                var displayNameAttr = type.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (displayNameAttr.Length > 0) {
                    properties.Add("Model", ((DisplayNameAttribute)displayNameAttr[0]).DisplayName.Trim());
                } else {
                    properties.Add("Model", type.Name);
                }

                var descriptionAttr = type.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (descriptionAttr.Length > 0) {
                    foreach (var property in ((DescriptionAttribute)descriptionAttr[0]).Description.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                        var keyValue = property.Split(new char[] { '=' }, 2);
                        var key = keyValue[0].Trim();
                        var value = (keyValue.Length > 1) ? keyValue[1].Trim() : "";
                        properties.Add(key, value);
                    }
                }

                DmmDriverCapabilities.PropertiesPerType.Add(type, properties);
                return properties;
            }
            return null;
        }

        private static String GetPropertyValue(Type type, String key) {
            var properties = GetProperties(type);
            if (properties != null) {
                String value;
                if (properties.TryGetValue(key, out value)) {
                    return value;
                }
            }
            return null;
        }

        #endregion

    }
}
