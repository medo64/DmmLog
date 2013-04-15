using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace DmmLogDriver {

    /// <summary>
    /// Base class for multimeter driver.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public abstract class DmmDriver : IDisposable {

        /// <summary>
        /// Creates new instance.
        /// </summary>
        protected DmmDriver() {
        }


        #region Communication

        /// <summary>
        /// Opens communication channel toward multimeter.
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// Closes communication channel toward multimeter.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Gets whether communication channel is still open.
        /// </summary>
        public abstract Boolean IsConnected { get; }

        #endregion


        #region Details

        /// <summary>
        /// Gets manufacturer and model combined.
        /// </summary>
        protected String DisplayName { get { return DmmDriverInformation.GetDriverInformation(this.GetType()).DisplayName; } }

        /// <summary>
        /// Gets all supported measurements.
        /// </summary>
        public abstract IEnumerable<DmmMeasurementType> SupportedMeasurements { get; }

        #endregion


        #region Queries

        /// <summary>
        /// Returns identificator for connected device.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The method potentially performs a time-consuming operation.")]
        public abstract DmmIdentification GetIdentification();


        /// <summary>
        /// Returns current measurement.
        /// In case of error, method should return null.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The method potentially performs a time-consuming operation.")]
        public abstract DmmMeasurement GetCurrentMeasurement();

        #endregion


        #region IDisposable

        /// <summary>
        /// Disposes used resources.
        /// </summary>
        /// <param name="disposing">True to dispose all resources; false to dispose only unmanaged.</param>
        protected virtual void Dispose(Boolean disposing) {
            if (disposing) {
                this.Disconnect();
            }
        }

        /// <summary>
        /// Disposes used resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
