using System;
using System.Collections.Generic;
using System.Text;

namespace DmmLogDriver {
    /// <summary>
    /// Multimeter Information.
    /// </summary>
    public class DmmInformation {

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="properties">Properties.</param>
        /// <exception cref="System.ArgumentNullException">Properties cannot be null.</exception>
        public DmmInformation(DmmPropertyDictionary properties) {
            if (properties == null) { throw new ArgumentNullException("properties", "Properties cannot be null."); }
            this.Properties = properties.AsReadOnly();
        }


        private readonly DmmPropertyDictionary Properties;


        /// <summary>
        /// Gets name of manufacturer.
        /// Null if name cannot be determined.
        /// </summary>
        public String Manufacturer { get { return GetStringProperty("Manufacturer"); } }

        /// <summary>
        /// Gets model name.
        /// Null if model cannot be determined.
        /// </summary>
        public String Model { get { return GetStringProperty("Model"); } }

        /// <summary>
        /// Gets serial number.
        /// Null if serial cannot be determined.
        /// </summary>
        public String Serial { get { return GetStringProperty("Serial"); } }

        /// <summary>
        /// Gets firmware version.
        /// Null if firmware version cannot be determined.
        /// </summary>
        public String FirmwareVersion { get { return GetStringProperty("FirmwareVersion"); } }

        /// <summary>
        /// Gets all properties.
        /// </summary>
        public DmmPropertyDictionary AllProperties { get { return this.Properties; } }


        /// <summary>
        /// Returns manufacturer and model name.
        /// </summary>
        public override string ToString() {
            var sb = new StringBuilder();
            if (this.Manufacturer != null) {
                sb.Append(this.Manufacturer);
            }
            if (this.Model != null) {
                if (sb.Length > 0) { sb.Append(" "); }
                sb.Append(this.Model);
            }
            if (sb.Length == 0) { sb.Append("Unknown"); }

            return sb.ToString();
        }


        #region Helper

        private string GetStringProperty(string key) {
            string value;
            return (this.Properties.TryGetValue(key, out value)) ? value : null as string;
        }

        #endregion

    }
}
