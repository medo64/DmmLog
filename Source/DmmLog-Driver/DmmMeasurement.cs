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
            : this(value, DmmMeasurementType.Unknown, 0) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Measurement value.</param>
        /// <param name="type">Measurement type.</param>
        /// <exception cref="System.ArgumentNullException">Type cannot be null.</exception>
        public DmmMeasurement(Decimal value, DmmMeasurementType type)
            : this(value, type, 0) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Measurement value.</param>
        /// <param name="type">Measurement type.</param>
        /// <param name="resolution">Measurement resolution (e.g. 0.001 for 1 mV resolution). If resolution is 0 then best suitable resolution will be found.</param>
        /// <exception cref="System.ArgumentNullException">Type cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Resolution cannot be negative.</exception>
        public DmmMeasurement(Decimal value, DmmMeasurementType type, Decimal resolution) {
            if (type == null) { throw new ArgumentNullException("type", "Type cannot be null."); }
            if (resolution < 0) { throw new ArgumentOutOfRangeException("resolution", "Resolution cannot be negative."); }
            this.Time = DateTime.UtcNow;
            this.Value = value;
            this.MeasurementType = type;
            this.Resolution = resolution;
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
        /// Gets measurement type.
        /// </summary>
        public DmmMeasurementType MeasurementType { get; private set; }

        /// <summary>
        /// Gets resolution of measurement (e.g. 0.01 if resolution was 10 mV).
        /// </summary>
        public Decimal Resolution { get; private set; }


        /// <summary>
        /// Gets exponent part for engineering value notation.
        /// If resolution is defined, exponent will match it.
        /// </summary>
        public Int32 EngineeringExponent {
            get { return GetEngineeringExponent((this.Resolution > 0) ? this.Resolution : this.Value); }
        }

        /// <summary>
        /// Gets coefficient part for engineering number notation.
        /// </summary>
        public Decimal EngineeringCoefficient {
            get {
                var value = this.Value;
                if ((value == decimal.MaxValue) || (value == decimal.MinValue)) { return value; }
                var exponent = this.EngineeringExponent;
                if (exponent >= 0) {
                    for (int i = 0; i < exponent; i++) {
                        value = value / 10;
                    }
                    return value;
                } else {
                    for (int i = exponent; i < 0; i++) {
                        value = value * 10;
                    }
                    return value;
                }
            }
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


        #region Helpers

        private static int GetEngineeringExponent(decimal value) {
            if ((value == decimal.MaxValue) || (value == decimal.MinValue)) { return 0; }
            value = Math.Abs(value);
            if (value == 0) {
                return 0;
            } else if (value >= 1) {
                var exp = 0;
                while (value > 10) {
                    value /= 10;
                    exp += 1;
                }
                exp = (exp / 3) * 3;
                return (exp > 9) ? 9 : exp;
            } else {
                var exp = 0;
                while (value < 1) {
                    value *= 10;
                    exp -= 1;
                }
                exp = ((exp - 2) / 3) * 3;
                return (exp < -9) ? -9 : exp;
            }
        }

        #endregion

    }
}
