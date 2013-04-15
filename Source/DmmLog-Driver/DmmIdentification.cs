using System;
using System.Text;

namespace DmmLogDriver {
    /// <summary>
    /// Identification data for multimeter.
    /// </summary>
    public class DmmIdentification {

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="manufacturer">Manufacturer name.</param>
        /// <param name="model">Model name.</param>
        public DmmIdentification(String manufacturer, String model)
            : this(manufacturer, model, null, null, null) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="manufacturer">Manufacturer name.</param>
        /// <param name="model">Model name.</param>
        /// <param name="serial">Serial number.</param>
        /// <param name="firmwareVersion">Firmware version.</param>
        public DmmIdentification(String manufacturer, String model, String serial, Version firmwareVersion) :
            this(manufacturer, model, serial, firmwareVersion, null) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="manufacturer">Manufacturer name.</param>
        /// <param name="model">Model name.</param>
        /// <param name="serial">Serial number.</param>
        /// <param name="firmwareVersion">Firmware version.</param>
        /// <param name="additionalComments">Additional comments.</param>
        public DmmIdentification(String manufacturer, String model, String serial, Version firmwareVersion, String additionalComments) {
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.Serial = serial;
            this.FirmwareVersion = firmwareVersion;
            this.AdditionalComments = additionalComments;
        }


        /// <summary>
        /// Gets name of manufacturer.
        /// Null if name cannot be determined.
        /// </summary>
        public String Manufacturer { get; private set; }

        /// <summary>
        /// Gets model name.
        /// Null if model cannot be determined.
        /// </summary>
        public String Model { get; private set; }

        /// <summary>
        /// Gets serial number.
        /// Null if serial cannot be determined.
        /// </summary>
        public String Serial { get; private set; }

        /// <summary>
        /// Gets firmware version.
        /// Null if firmware version cannot be determined.
        /// </summary>
        public Version FirmwareVersion { get; private set; }

        /// <summary>
        /// Gets additional comments.
        /// Null if there aren't any.
        /// </summary>
        public String AdditionalComments { get; private set; }

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

    }
}
