using System;

namespace DmmLogDriver {

    /// <summary>
    /// Driver interface type.
    /// </summary>
    public enum DmmDriverInterface {
        /// <summary>
        /// Driver does not need any connection.
        /// </summary>
        None = 0,

        /// <summary>
        /// Driver uses serial connection.
        /// </summary>
        SerialPort = 1
    }
}
