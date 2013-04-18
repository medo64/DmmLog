using System;
using System.Diagnostics;

namespace DmmLogDriver.Helpers {
    internal static class EngineeringNotation {

        internal static Int32 GetEngineeringExponent(Decimal value, Int32 minExponent, Int32 maxExponent) {
            Decimal coefficient;
            Int32 exponent;
            GetNumbers(value, out coefficient, out exponent, minExponent, maxExponent);
            return exponent;
        }

        internal static Decimal GetEngineeringCoefficient(Decimal value, Int32 minExponent, Int32 maxExponent) {
            Decimal coefficient;
            Int32 exponent;
            GetNumbers(value, out coefficient, out exponent, minExponent, maxExponent);
            return coefficient;
        }

        internal static void GetNumbers(Decimal value, out Decimal coefficient, out Int32 exponent, Int32 minExponent, Int32 maxExponent) {
            Debug.Assert(minExponent % 3 == 0);
            Debug.Assert(minExponent >= MinEngineeringExponent);
            Debug.Assert(maxExponent % 3 == 0);
            Debug.Assert(maxExponent <= MaxEngineeringExponent);

            if (value == decimal.MinValue) {
                coefficient = decimal.MinValue;
                exponent = minExponent;
            } else if (value == decimal.MaxValue) {
                coefficient = decimal.MaxValue;
                exponent = maxExponent;
            } else if (value == 0) {
                coefficient = 0;
                exponent = minExponent;
            } else {
                var valueExp = Math.Abs(value);
                if (valueExp >= 1) {
                    exponent = 0;
                    while ((valueExp >= 10) && (exponent < maxExponent)) {
                        valueExp /= 10;
                        exponent += 1;
                    }
                    exponent = (exponent / 3) * 3;

                    for (int i = 0; i < exponent; i++) {
                        value = value / 10;
                    }
                    coefficient = value;
                } else {
                    exponent = 0;
                    while ((valueExp < 1) && (exponent > minExponent)) {
                        valueExp *= 10;
                        exponent -= 1;
                    }
                    exponent = ((exponent - 2) / 3) * 3;

                    for (int i = exponent; i < 0; i++) {
                        value = value * 10;
                    }
                    coefficient = value;
                }
            }
        }


        internal static int MinEngineeringExponent { get { return -9; } }
        internal static int MaxEngineeringExponent { get { return 9; } }

    }
}
