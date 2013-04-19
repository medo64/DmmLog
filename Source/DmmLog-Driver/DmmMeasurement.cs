using System;
using System.Globalization;

namespace DmmLogDriver {
    /// <summary>
    /// Stores single multimeter measurement.
    /// </summary>
    public class DmmMeasurement {

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Measurement value.</param>
        public DmmMeasurement(Decimal value)
            : this(value, DmmMeasurementType.Unknown) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Measurement value.</param>
        /// <param name="type">Measurement type.</param>
        /// <exception cref="System.ArgumentNullException">Type cannot be null.</exception>
        public DmmMeasurement(Decimal value, DmmMeasurementType type) {
            if (type == null) { throw new ArgumentNullException("type", "Type cannot be null."); }
            this.Time = DateTime.UtcNow;
            this.Value = new DmmEngineeringNotation(value);
            this.MeasurementRange = new DmmMeasurementRange(type);
            this.MeasurementType = type;
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Measurement value.</param>
        /// <param name="range">Measurement range.</param>
        /// <exception cref="System.ArgumentNullException">Range cannot be null.</exception>
        public DmmMeasurement(Decimal value, DmmMeasurementRange range) {
            if (range == null) { throw new ArgumentNullException("range", "Range cannot be null."); }
            this.Time = DateTime.UtcNow;
            this.Value = new DmmEngineeringNotation(value, range.MinimumExponent, range.MaximumExponent);
            this.MeasurementRange = range;
            this.MeasurementType = range.MeasurementType;
        }


        /// <summary>
        /// Gets time when measurement was created.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Gets value of measurement in SI units.
        /// </summary>
        public DmmEngineeringNotation Value { get; private set; }

        /// <summary>
        /// Gets measurement range.
        /// </summary>
        public DmmMeasurementRange MeasurementRange { get; private set; }

        /// <summary>
        /// Gets measurement type.
        /// </summary>
        public DmmMeasurementType MeasurementType { get; private set; }


        /// <summary>
        /// Returns value scaled for SI range.
        /// </summary>
        public Decimal SIValue {
            get { return this.Value.Coefficient; }
        }

        /// <summary>
        /// Gets SI unit for value.
        /// </summary>
        public String SIUnit {
            get { return string.Format(CultureInfo.CurrentCulture, "{0}{1}", this.Value.SIPrefix, this.Unit); }
        }

        /// <summary>
        /// Gets unit for value.
        /// </summary>
        public String Unit {
            get { return this.MeasurementType.Unit; }
        }

        /// <summary>
        /// Gets unit marking for value (e.g. ~ for AC).
        /// </summary>
        public String ExtraMarking {
            get { return this.MeasurementRange.ExtraMarking; }
        }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString() {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        public String ToString(IFormatProvider provider) {
            return string.Format(provider, "{0:0.######} {1}", this.SIValue, this.SIUnit).Trim();
        }

    }
}
