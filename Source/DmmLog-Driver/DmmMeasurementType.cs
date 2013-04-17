using System;

namespace DmmLogDriver {

    /// <summary>
    /// List of all supported measurements.
    /// </summary>
    public class DmmMeasurementType {

        private DmmMeasurementType(String id, String title, String unit) {
            this.Id = id;
            this.Title = title;
            this.Unit = unit;
        }


        /// <summary>
        /// Gets unique identificator for given measurement type.
        /// </summary>
        public String Id { get; private set; }

        /// <summary>
        /// Gets user friendly name for measurement.
        /// </summary>
        public String Title { get; private set; }

        /// <summary>
        /// Gets unit for measurement.
        /// </summary>
        public String Unit { get; private set; }


        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object. to compare with the current object.</param>
        public override bool Equals(object obj) {
            var other = obj as DmmMeasurementType;
            return (other != null) && (other.Id.Equals(this.Id, StringComparison.Ordinal));
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode() {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString() {
            return this.Title;
        }



        #region Static

        private static DmmMeasurementType _unknown = new DmmMeasurementType(null, "Unknown", null);
        /// <summary>
        /// Gets voltage (DC) measurement type.
        /// </summary>
        public static DmmMeasurementType Unknown { get { return _unknown; } }


        private static DmmMeasurementType _voltageDC = new DmmMeasurementType("VoltageDC", "Voltage (DC)", "V");
        /// <summary>
        /// Gets voltage (DC) measurement type.
        /// </summary>
        public static DmmMeasurementType VoltageDC { get { return _voltageDC; } }

        private static DmmMeasurementType _voltageAC = new DmmMeasurementType("VoltageAC", "Voltage (AC)", "V~");
        /// <summary>
        /// Gets voltage (AC) measurement type.
        /// </summary>
        public static DmmMeasurementType VoltageAC { get { return _voltageAC; } }

        private static DmmMeasurementType _resistance = new DmmMeasurementType("Resistance", "Resistance", "Ω");
        /// <summary>
        /// Gets resistance measurement type.
        /// </summary>
        public static DmmMeasurementType Resistance { get { return _resistance; } }

        private static DmmMeasurementType _diode = new DmmMeasurementType("Diode", "Diode", "V");
        /// <summary>
        /// Gets diode forward voltage measurement type.
        /// </summary>
        public static DmmMeasurementType Diode { get { return _diode; } }

        private static DmmMeasurementType _capacitance = new DmmMeasurementType("Capacitance", "Capacitance", "F");
        /// <summary>
        /// Gets capacitance measurement type.
        /// </summary>
        public static DmmMeasurementType Capacitance { get { return _capacitance; } }

        private static DmmMeasurementType _currentDC = new DmmMeasurementType("CurrentDC", "Current (DC)", "A");
        /// <summary>
        /// Gets current (DC) measurement type.
        /// </summary>
        public static DmmMeasurementType CurrentDC { get { return _currentDC; } }

        private static DmmMeasurementType _currentAC = new DmmMeasurementType("CurrentAC", "Current (AC)", "A~");
        /// <summary>
        /// Gets current (AC) measurement type.
        /// </summary>
        public static DmmMeasurementType CurrentAC { get { return _currentAC; } }

        private static DmmMeasurementType _frequency = new DmmMeasurementType("Frequency", "Frequency", "Hz");
        /// <summary>
        /// Gets frequency measurement type.
        /// </summary>
        public static DmmMeasurementType Frequency { get { return _frequency; } }

        #endregion

    }
}
