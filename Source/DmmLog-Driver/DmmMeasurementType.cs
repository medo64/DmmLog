using System;
using System.Collections.Generic;

namespace DmmLogDriver {

    /// <summary>
    /// List of all supported measurements.
    /// </summary>
    public class DmmMeasurementType {

        private DmmMeasurementType(String key, String title, String unit) {
            this.Key = key;
            this.Title = title;
            this.Unit = unit;
        }


        /// <summary>
        /// Gets unique identificator for given measurement type.
        /// </summary>
        public String Key { get; private set; }

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
            return (other != null) && (other.Key.Equals(this.Key, StringComparison.Ordinal));
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode() {
            return this.Key.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString() {
            return this.Title;
        }



        #region Static

        private static Dictionary<String, DmmMeasurementType> Dictionary;

        /// <summary>
        /// Returns measurement type based on key or null if lookup fails.
        /// </summary>
        /// <param name="key">Lookup key.</param>
        public static DmmMeasurementType GetMeasurementType(string key) {
            if (DmmMeasurementType.Dictionary == null) {
                var dict = new Dictionary<String, DmmMeasurementType>(StringComparer.OrdinalIgnoreCase);
                AddToDictionary(dict, "", "Unknown", "");
                AddToDictionary(dict, "VoltageDC", "Voltage (DC)", "V");
                AddToDictionary(dict, "VoltageAC", "Voltage (AC)", "V");
                AddToDictionary(dict, "Resistance", "Resistance", "Ω");
                AddToDictionary(dict, "Diode", "Diode", "V");
                AddToDictionary(dict, "Capacitance", "Capacitance", "F");
                AddToDictionary(dict, "CurrentDC", "Current (DC)", "A");
                AddToDictionary(dict, "CurrentAC", "Current (AC)", "A");
                AddToDictionary(dict, "Frequency", "Frequency", "Hz");
                AddToDictionary(dict, "Temperature", "Temperature", "°C");
                DmmMeasurementType.Dictionary = dict;
            }
            DmmMeasurementType type;
            if (DmmMeasurementType.Dictionary.TryGetValue(key, out type)) {
                return type;
            } else {
                return null;
            }
        }

        private static void AddToDictionary(Dictionary<String, DmmMeasurementType> dictionary, String key, String title, String unit) {
            dictionary.Add(key, new DmmMeasurementType(key, title, unit));
        }


        /// <summary>
        /// Gets unknown measurement type.
        /// </summary>
        public static DmmMeasurementType Unknown { get { return DmmMeasurementType.GetMeasurementType(""); } }

        /// <summary>
        /// Gets voltage (DC) measurement type.
        /// </summary>
        public static DmmMeasurementType VoltageDC { get { return DmmMeasurementType.GetMeasurementType("VoltageDC"); } }

        /// <summary>
        /// Gets voltage (AC) measurement type.
        /// </summary>
        public static DmmMeasurementType VoltageAC { get { return DmmMeasurementType.GetMeasurementType("VoltageAC"); } }

        /// <summary>
        /// Gets resistance measurement type.
        /// </summary>
        public static DmmMeasurementType Resistance { get { return DmmMeasurementType.GetMeasurementType("Resistance"); } }

        /// <summary>
        /// Gets diode forward voltage measurement type.
        /// </summary>
        public static DmmMeasurementType Diode { get { return DmmMeasurementType.GetMeasurementType("Diode"); } }

        /// <summary>
        /// Gets capacitance measurement type.
        /// </summary>
        public static DmmMeasurementType Capacitance { get { return DmmMeasurementType.GetMeasurementType("Capacitance"); } }

        /// <summary>
        /// Gets current (DC) measurement type.
        /// </summary>
        public static DmmMeasurementType CurrentDC { get { return DmmMeasurementType.GetMeasurementType("CurrentDC"); } }

        /// <summary>
        /// Gets current (AC) measurement type.
        /// </summary>
        public static DmmMeasurementType CurrentAC { get { return DmmMeasurementType.GetMeasurementType("CurrentAC"); } }

        /// <summary>
        /// Gets frequency measurement type.
        /// </summary>
        public static DmmMeasurementType Frequency { get { return DmmMeasurementType.GetMeasurementType("Frequency"); } }

        /// <summary>
        /// Gets temperature measurement type.
        /// </summary>
        public static DmmMeasurementType Temperature { get { return DmmMeasurementType.GetMeasurementType("Temperature"); } }

        #endregion

    }
}
