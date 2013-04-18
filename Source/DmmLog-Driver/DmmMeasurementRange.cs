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
            : this(title, EngineeringNotation.MinEngineeringExponent, EngineeringNotation.MaxEngineeringExponent, measurementType) {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="title">Title of range (e.g. 60mV).</param>
        /// <param name="minimumExponent">Minimum exponent value.</param>
        /// <param name="maximumExponent">Maximum exponent value.</param>
        /// <param name="measurementType">Measurement type for range.</param>\
        /// <exception cref="System.ArgumentNullException">Title cannot be null. -or- Measurement type cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Maximum must be larger than or equal to minimum.</exception>
        public DmmMeasurementRange(String title, Int32 minimumExponent, Int32 maximumExponent, DmmMeasurementType measurementType) {
            if (title == null) { throw new ArgumentNullException("title", "Title cannot be null."); }
            if (measurementType == null) { throw new ArgumentNullException("measurementType", "Measurement type cannot be null."); }
            if (minimumExponent < EngineeringNotation.MinEngineeringExponent) { minimumExponent = EngineeringNotation.MinEngineeringExponent; }
            if (maximumExponent > EngineeringNotation.MaxEngineeringExponent) { maximumExponent = EngineeringNotation.MaxEngineeringExponent; }
            if (maximumExponent < minimumExponent) { throw new ArgumentOutOfRangeException("maximumExponent", "Maximum must be larger than or equal to minimum."); }

            this.Title = title;
            this.MinimumExponent = minimumExponent;
            this.MaximumExponent = maximumExponent;
            this.MeasurementType = measurementType;
        }

        /// <summary>
        /// Gets title.
        /// </summary>
        public String Title { get; private set; }

        /// <summary>
        /// Gets minimum engineering exponent.
        /// </summary>
        public Int32 MinimumExponent { get; private set; }

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
