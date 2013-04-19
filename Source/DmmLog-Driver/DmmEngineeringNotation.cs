using System;
using System.Globalization;

namespace DmmLogDriver {
    /// <summary>
    /// Dealing with values using engineering notation.
    /// </summary>
    public struct DmmEngineeringNotation {

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Value.</param>
        public DmmEngineeringNotation(Decimal value)
            : this(value, DmmEngineeringNotation.MinimumExponent, DmmEngineeringNotation.MaximumExponent) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="minimumExponent">Minimum exponent value.</param>
        /// <param name="maximumExponent">Maximum exponent value.</param>
        public DmmEngineeringNotation(Decimal value, Int32 minimumExponent, Int32 maximumExponent)
            : this() {
            if (minimumExponent < DmmEngineeringNotation.MinimumExponent) { minimumExponent = DmmEngineeringNotation.MinimumExponent; }
            if (maximumExponent > DmmEngineeringNotation.MaximumExponent) { maximumExponent = DmmEngineeringNotation.MaximumExponent; }
            if (maximumExponent < minimumExponent) { throw new ArgumentOutOfRangeException("maximumExponent", "Maximum must be larger than or equal to minimum."); }

            this.Value = value;

            if (value == decimal.MinValue) {
                this.Exponent = minimumExponent;
            } else if (value == decimal.MaxValue) {
                this.Exponent = maximumExponent;
            } else if (value == 0) {
                this.Exponent = minimumExponent;
            } else {
                value = Math.Abs(value);
                if (value >= 1) {
                    var exponent = 0;
                    while ((value >= 10) && (exponent < maximumExponent)) {
                        value /= 10;
                        exponent += 1;
                    }
                    this.Exponent = (exponent / 3) * 3;
                } else {
                    var exponent = 0;
                    while ((value < 1) && (exponent > minimumExponent)) {
                        value *= 10;
                        exponent -= 1;
                    }
                    this.Exponent = ((exponent - 2) / 3) * 3;
                }
            }
        }


        /// <summary>
        /// Gets value.
        /// </summary>
        public Decimal Value { get; private set; }

        /// <summary>
        /// Gets exponent in engineering notation.
        /// </summary>
        public Int32 Exponent { get; private set; }

        /// <summary>
        /// Gets coefficient for given exponent in engineering notation.
        /// </summary>
        public Decimal Coefficient {
            get {
                var value = this.Value;
                if ((value == decimal.MinValue) || (value == 0) || (value == decimal.MaxValue)) {
                    return value;
                } else if (this.Exponent >= 0) {
                    for (int i = 0; i < this.Exponent; i++) {
                        value = value / 10;
                    }
                    return value;
                } else {
                    for (int i = this.Exponent; i < 0; i++) {
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
                switch (this.Exponent) {
                    case -24: return "y"; //yocto
                    case -21: return "z"; //zepto
                    case -18: return "a"; //atto
                    case -15: return "f"; //femto
                    case -12: return "p"; //piko
                    case -9: return "n"; //nano
                    case -6: return "µ"; //micro
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
                    default: throw new InvalidOperationException("Cannot determine SI prefix for exponent " + this.Exponent.ToString(CultureInfo.InvariantCulture) + ".");
                }
            }
        }


        /// <summary>
        /// Returns a value indicating whether this instance and a object represent the same type and value.
        /// </summary>
        /// <param name="obj">An object.</param>
        public override Boolean Equals(object obj) {
            if (obj is DmmEngineeringNotation) {
                return this.Value == ((DmmEngineeringNotation)obj).Value;
            } else if (obj is decimal) {
                return this.Value == (decimal)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override Int32 GetHashCode() {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        public override String ToString() {
            return this.Value.ToString(CultureInfo.CurrentCulture);
        }


        #region Operators

        /// <summary>
        /// Returns a value indicating whether two instances are equal.
        /// </summary>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        public static bool operator ==(DmmEngineeringNotation value1, DmmEngineeringNotation value2) {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Returns a value indicating whether two instances are not equal.
        /// </summary>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        public static bool operator !=(DmmEngineeringNotation value1, DmmEngineeringNotation value2) {
            return !value1.Equals(value2);
        }


        /// <summary>
        /// Returns integer value of DmmEngineeringNotation object.
        /// </summary>
        /// <param name="obj">Object.</param>
        public static implicit operator int(DmmEngineeringNotation obj) {
            return (int)obj.Value;
        }

        /// <summary>
        /// Returns decimal value of DmmEngineeringNotation object.
        /// </summary>
        /// <param name="obj">Object.</param>
        public static implicit operator decimal(DmmEngineeringNotation obj) {
            return obj.Value;
        }

        /// <summary>
        /// Returns double value of DmmEngineeringNotation object.
        /// </summary>
        /// <param name="obj">Object.</param>
        public static implicit operator double(DmmEngineeringNotation obj) {
            return (double)obj.Value;
        }

        #endregion


        /// <summary>
        /// Gets minimum allowable exponent.
        /// </summary>
        public static Int32 MinimumExponent { get { return -9; } }

        /// <summary>
        /// Gets maximum allowable exponent.
        /// </summary>
        public static Int32 MaximumExponent { get { return 9; } }

    }
}
