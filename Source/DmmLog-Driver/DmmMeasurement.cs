using DmmLogDriver.Helpers;
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
            this.Value = value;
            this.MeasurementRange = new DmmMeasurementRange(type.Title, type);
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
            this.Value = value;
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
        public Decimal Value { get; private set; }

        /// <summary>
        /// Gets measurement range.
        /// </summary>
        public DmmMeasurementRange MeasurementRange { get; private set; }

        /// <summary>
        /// Gets measurement type.
        /// </summary>
        public DmmMeasurementType MeasurementType { get; private set; }


        /// <summary>
        /// Gets exponent part for engineering value notation.
        /// </summary>
        public Int32 EngineeringExponent {
            get { return EngineeringNotation.GetEngineeringExponent(this.Value, this.MeasurementRange.MinimumExponent, this.MeasurementRange.MaximumExponent); }
        }

        /// <summary>
        /// Gets coefficient part for engineering number notation.
        /// </summary>
        public Decimal EngineeringCoefficient {
            get { return EngineeringNotation.GetEngineeringCoefficient(this.Value, this.MeasurementRange.MinimumExponent, this.MeasurementRange.MaximumExponent); }
        }

        /// <summary>
        /// Gets SI prefix for value.
        /// </summary>
        public String SIPrefix {
            get {
                var exp = this.EngineeringExponent;
                switch (exp) {
                    case -24: return "y"; //yocto
                    case -21: return "z"; //zepto
                    case -18: return "a"; //atto
                    case -15: return "f"; //femto
                    case -12: return "p"; //piko
                    case -9: return "n"; //nano
                    case -6: return "μ"; //micro
                    case -3: return "m"; //milli
                    case 0: return "";
                    case 3: return "k"; //kilo
                    case 6: return "M"; //mega
                    case 9: return "G"; //giga
                    case 12: return "T"; //tera
                    case 15: return "P"; //peta
                    case 18: return "E"; //exa
                    case 21: return "Z"; //zetta
                    case 24: return "Y"; //yotta
                    default: throw new InvalidOperationException("Cannot determine SI prefix for " + exp.ToString(CultureInfo.InvariantCulture) + ".");
                }

            }
        }

        /// <summary>
        /// Gets SI unit for value.
        /// </summary>
        public String SIUnit {
            get { return string.Format(CultureInfo.CurrentCulture, "{0}{1}", this.SIPrefix, this.MeasurementType.Unit); }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString() {
            return string.Format(CultureInfo.CurrentCulture, "{0:0.######} {1}", this.EngineeringCoefficient, this.SIUnit);
        }

    }
}
