using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;

namespace DmmLogDriver {
    /// <summary>
    /// Storage for serial port settings.
    /// </summary>
    public class DmmSerialPortSettings {

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public DmmSerialPortSettings()
            : this("COM1", 9600, Parity.None, 8, StopBits.One) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="portName">Port name.</param>
        /// <param name="baudRate">Baud rate.</param>
        /// <param name="parity">Parity.</param>
        /// <param name="dataBits">Data bits.</param>
        /// <param name="stopBits">Stop bits.</param>
        /// <exception cref="System.ArgumentNullException">Port name cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Port name must start with COM. -or- Port name must contain port number. -or- Unknown parity value. -or- Unknown data bits value. -or- Unknown stop bits value.</exception>
        public DmmSerialPortSettings(String portName, Int32 baudRate, Parity parity, Int32 dataBits, StopBits stopBits) {
            if (portName == null) { throw new ArgumentNullException("portName", "Port name cannot be null."); }
            portName = portName.Trim();
            if (!(portName.StartsWith("COM", StringComparison.OrdinalIgnoreCase))) { throw new ArgumentOutOfRangeException("portName", "Port name must start with COM."); }
            if (portName.EndsWith(":", StringComparison.Ordinal)) { portName = portName.Substring(0, portName.Length - 1).Trim(); }
            Int32 portNumber;
            if (!(int.TryParse(portName.Substring(3), NumberStyles.Integer, CultureInfo.InvariantCulture, out portNumber) && (portNumber > 0))) { throw new ArgumentOutOfRangeException("portName", "Port name must contain positive port number."); }

            var sb = new StringBuilder();

            this.PortName = "COM" + portNumber.ToString(CultureInfo.InvariantCulture);
            sb.Append(this.PortName);
            sb.Append(":");

            this.BaudRate = baudRate;
            sb.Append(this.BaudRate.ToString(CultureInfo.InvariantCulture));
            sb.Append(",");

            this.Parity = parity;
            switch (this.Parity) {
                case Parity.None: sb.Append("N"); break;
                case Parity.Even: sb.Append("E"); break;
                case Parity.Odd: sb.Append("O"); break;
                case Parity.Mark: sb.Append("M"); break;
                case Parity.Space: sb.Append("S"); break;
                default: throw new ArgumentOutOfRangeException("parity", "Unknown parity value.");
            }
            sb.Append(",");

            this.DataBits = dataBits;
            if ((this.DataBits < 5) || (this.DataBits > 8)) { throw new ArgumentOutOfRangeException("dataBits", "Unknown data bits value."); }
            sb.Append(this.DataBits.ToString(CultureInfo.InvariantCulture));
            sb.Append(",");

            this.StopBits = stopBits;
            switch (this.StopBits) {
                case StopBits.None: sb.Append("0"); break;
                case StopBits.One: sb.Append("1"); break;
                case StopBits.OnePointFive: sb.Append("1.5"); break;
                case StopBits.Two: sb.Append("2"); break;
                default: throw new ArgumentOutOfRangeException("stopBits", "Unknown stop bits value.");
            }

            this.Settings = sb.ToString();
        }


        /// <summary>
        /// Gets port name.
        /// </summary>
        public String PortName { get; private set; }

        /// <summary>
        /// Gets baud rate.
        /// </summary>
        public Int32 BaudRate { get; private set; }

        /// <summary>
        /// Gets parity.
        /// </summary>
        public Parity Parity { get; private set; }

        /// <summary>
        /// Gets data bits.
        /// </summary>
        public Int32 DataBits { get; private set; }

        /// <summary>
        /// Gets stop bits.
        /// </summary>
        public StopBits StopBits { get; private set; }


        private readonly String Settings;


        /// <summary>
        /// Returns settings string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return this.Settings;
        }


        /// <summary>
        /// Returns parsed value.
        /// </summary>
        /// <param name="settings">String in following format "portName:baudrate,parity,dataBits,stopBits" (e.g. "COM1:9600,N,8,1").</param>
        public static DmmSerialPortSettings Parse(string settings) {
            try {
                if (settings != null) {
                    var parts = settings.Split(':');
                    if ((parts.Length == 1) || (parts.Length == 2)) {
                        var portName = parts[0].Length > 0 ? parts[0].Trim().ToUpperInvariant() : "COM1";
                        Int32 baudRate;
                        Parity parity;
                        Int32 dataBits;
                        StopBits stopBits;
                        if (ParseSerialAuxSettings(parts.Length == 2 ? parts[1] : null, out baudRate, out parity, out dataBits, out stopBits)) {
                            return new DmmSerialPortSettings(portName, baudRate, parity, dataBits, stopBits);
                        }
                    }
                }
            } catch (ArgumentOutOfRangeException ex) {
                throw new FormatException(ex.Message, ex);
            }
            throw new FormatException("Input string (" + settings + ") is not in valid format.");
        }


        private static Boolean ParseSerialAuxSettings(String auxSettings, out Int32 baudRate, out Parity parity, out Int32 dataBits, out StopBits stopBits) {
            baudRate = 9600;
            parity = Parity.None;
            dataBits = 8;
            stopBits = StopBits.One;

            if (auxSettings == null) { return true; } //just return defaults

            var parts = auxSettings.ToUpperInvariant().Split(',');

            var baudRateText = (parts.Length > 0) ? parts[0].Trim() : null;
            if (!(string.IsNullOrEmpty(baudRateText) || int.TryParse(baudRateText, NumberStyles.Integer, CultureInfo.InvariantCulture, out baudRate))) {
                return false;
            }

            var parityText = (parts.Length > 1) ? parts[1].Trim() : null;
            if (!(string.IsNullOrEmpty(parityText))) {
                switch (parityText) {
                    case "N": parity = Parity.None; break;
                    case "O": parity = Parity.Odd; break;
                    case "E": parity = Parity.Even; break;
                    case "M": parity = Parity.Mark; break;
                    case "S": parity = Parity.Space; break;
                    default: return false;
                }
            }

            var dataBitsText = (parts.Length > 2) ? parts[2].Trim() : null;
            if (!(string.IsNullOrEmpty(dataBitsText) || int.TryParse(dataBitsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out dataBits))) {
                return false;
            }
            if ((dataBits < 5) || (dataBits > 8)) { return false; }

            var stopBitsText = (parts.Length > 3) ? parts[3].Trim() : null;
            if (!(string.IsNullOrEmpty(stopBitsText))) {
                switch (stopBitsText) {
                    case "0": stopBits = StopBits.None; break;
                    case "1": stopBits = StopBits.One; break;
                    case "1.5": stopBits = StopBits.OnePointFive; break;
                    case "2": stopBits = StopBits.Two; break;
                    default: return false;
                }
            }

            return true;
        }

    }
}
