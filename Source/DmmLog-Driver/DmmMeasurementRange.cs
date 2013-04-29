using System;
using System.Diagnostics;

namespace DmmLogDriver {
    /// <summary>
    /// Multimeter range.
    /// </summary>
    [DebuggerDisplay("{Title}")]
    public class DmmMeasurementRange {

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="measurementType">Measurement type for range.</param>\
        /// <exception cref="System.ArgumentNullException">Title cannot be null. -or- Measurement type cannot be null.</exception>
        public DmmMeasurementRange(DmmMeasurementType measurementType)
            : this((measurementType != null) ? measurementType.Title : null, DmmEngineeringNotation.MinimumExponent, DmmEngineeringNotation.MaximumExponent, measurementType, null) {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="measurementType">Measurement type for range.</param>\
        /// <param name="extraMarking">Extra marking (e.g. ~ for AC).</param>
        /// <exception cref="System.ArgumentNullException">Title cannot be null. -or- Measurement type cannot be null.</exception>
        public DmmMeasurementRange(DmmMeasurementType measurementType, String extraMarking)
            : this((measurementType != null) ? measurementType.Title : null, DmmEngineeringNotation.MinimumExponent, DmmEngineeringNotation.MaximumExponent, measurementType, extraMarking) {
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
        public DmmMeasurementRange(String title, Int32 minimumExponent, Int32 maximumExponent, DmmMeasurementType measurementType) :
            this(title, minimumExponent, maximumExponent, measurementType, null) {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="title">Title of range (e.g. 60mV).</param>
        /// <param name="minimumExponent">Minimum exponent value.</param>
        /// <param name="maximumExponent">Maximum exponent value.</param>
        /// <param name="measurementType">Measurement type for range.</param>
        /// <param name="extraMarking">Extra marking (e.g. ~ for AC).</param>
        /// <exception cref="System.ArgumentNullException">Title cannot be null. -or- Measurement type cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Maximum must be larger than or equal to minimum.</exception>
        public DmmMeasurementRange(String title, Int32 minimumExponent, Int32 maximumExponent, DmmMeasurementType measurementType, String extraMarking) {
            if (measurementType == null) { throw new ArgumentNullException("measurementType", "Measurement type cannot be null."); }
            if (title == null) { throw new ArgumentNullException("title", "Title cannot be null."); }

            minimumExponent = ((Math.Abs(minimumExponent) + 2) / 3 * 3) * Math.Sign(minimumExponent);
            maximumExponent = ((Math.Abs(maximumExponent) + 2) / 3 * 3) * Math.Sign(maximumExponent);
            if (minimumExponent < DmmEngineeringNotation.MinimumExponent) { minimumExponent = DmmEngineeringNotation.MinimumExponent; }
            if (maximumExponent > DmmEngineeringNotation.MaximumExponent) { maximumExponent = DmmEngineeringNotation.MaximumExponent; }
            if (maximumExponent < minimumExponent) { throw new ArgumentOutOfRangeException("maximumExponent", "Maximum must be larger than or equal to minimum."); }

            this.Title = title;
            this.MinimumExponent = minimumExponent;
            this.MaximumExponent = maximumExponent;
            this.MeasurementType = measurementType;
            this.ExtraMarking = extraMarking;
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

        /// <summary>
        /// Gets extra marking (e.g. ~ for AC).
        /// </summary>
        public String ExtraMarking { get; private set; }

    }
}
