using DmmLogDriver.Helpers;
using System;

namespace DmmLogDriver {
    /// <summary>
    /// Multimeter range.
    /// </summary>
    public class DmmMeasurementRange {

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="title">Title of range (e.g. 60mV).</param>
        /// <param name="measurementType">Measurement type for range.</param>\
        /// <exception cref="System.ArgumentNullException">Title cannot be null. -or- Measurement type cannot be null.</exception>
        public DmmMeasurementRange(String title, DmmMeasurementType measurementType)
            : this(title, decimal.MinValue, decimal.MaxValue, measurementType) {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="title">Title of range (e.g. 60mV).</param>
        /// <param name="minimumValue">Minimum value for range.</param>
        /// <param name="maximumValue">Maximum value for range.</param>
        /// <param name="measurementType">Measurement type for range.</param>\
        /// <exception cref="System.ArgumentNullException">Title cannot be null. -or- Measurement type cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Maximum must be larger than minimum.</exception>
        public DmmMeasurementRange(String title, Decimal minimumValue, Decimal maximumValue, DmmMeasurementType measurementType) {
            if (title == null) { throw new ArgumentNullException("title", "Title cannot be null."); }
            if (minimumValue > maximumValue) { throw new ArgumentOutOfRangeException("maximumValue", "Maximum must be larger than minimum."); }
            if (measurementType == null) { throw new ArgumentNullException("measurementType", "Measurement type cannot be null."); }

            this.Title = title;
            this.MinimumValue = minimumValue;
            this.MinimumExponent = (minimumValue > decimal.MinValue) ? EngineeringNotation.GetEngineeringExponent(minimumValue) : EngineeringNotation.MinEngineeringExponent;
            this.MaximumValue = maximumValue;
            this.MaximumExponent = (maximumValue < decimal.MaxValue) ? EngineeringNotation.GetEngineeringExponent(maximumValue) : EngineeringNotation.MaxEngineeringExponent;
            this.MeasurementType = measurementType;
        }

        /// <summary>
        /// Gets title.
        /// </summary>
        public String Title { get; private set; }

        /// <summary>
        /// Gets minimum value.
        /// </summary>
        public Decimal MinimumValue { get; private set; }

        /// <summary>
        /// Gets minimum engineering exponent.
        /// </summary>
        public Int32 MinimumExponent { get; private set; }

        /// <summary>
        /// Gets maximum value.
        /// </summary>
        public Decimal MaximumValue { get; private set; }

        /// <summary>
        /// Gets maximum engineering exponent.
        /// </summary>
        public Int32 MaximumExponent { get; private set; }

        /// <summary>
        /// Gets measurement type value.
        /// </summary>
        public DmmMeasurementType MeasurementType { get; private set; }

    }
}
