using DmmLogDriver;
using Medo.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace DmmLogDriverAgilent {
    [DebuggerDisplay("{DisplayName} @ {SerialPort.PortName}")]
    public abstract class AgilentBase : DmmDriver {
        protected AgilentBase(String settings)
            : base() {
            var serialPortSettings = DmmSerialPortSettings.Parse(settings);
            if (serialPortSettings.BaudRate != 9600) { throw new ArgumentOutOfRangeException("settings", "Device only supports baud rate of 9600."); }
            if (serialPortSettings.Parity != Parity.None) { throw new ArgumentOutOfRangeException("settings", "Device only supports no parity."); }
            if (serialPortSettings.DataBits != 8) { throw new ArgumentOutOfRangeException("settings", "Device only supports 8 data bits."); }
            if (serialPortSettings.StopBits != StopBits.One) { throw new ArgumentOutOfRangeException("settings", "Device only supports 1 stop bit."); }

            this.SerialPort = new Medo.IO.UartPort(serialPortSettings.PortName, serialPortSettings.BaudRate, serialPortSettings.Parity, serialPortSettings.DataBits, serialPortSettings.StopBits);
            this.SerialPort.ReadTimeout = 250;
            this.SerialPort.WriteTimeout = 250;
            this.SerialPort.NewLine = "\n";
        }


        #region Communication

        protected UartPort SerialPort { get; private set; }

        public override void Connect() {
            try {
                this.SerialPort.Open();
                this.SerialPort.Purge();
            } catch (IOException) { }
        }

        public override void Disconnect() {
            if (this.SerialPort.IsOpen) {
                try {
                    this.SerialPort.Purge();
                } catch (IOException) { }
                try {
                    this.SerialPort.Close();
                } catch (IOException) { }
            }
        }

        public override Boolean IsConnected {
            get { return this.SerialPort.IsOpen; }
        }


        private readonly object SyncUart = new object();

        private String SendScpi(string command) {
            if (this.IsConnected == false) { return null; }

            lock (this.SyncUart) { //to avoid other thread stealing result
                while (this.SerialPort.BytesToRead > 0) {
                    this.SerialPort.DiscardInBuffer();
                }

                this.SerialPort.Write(ASCIIEncoding.ASCII.GetBytes(command + "\n"));

                try {
                    var result = this.SerialPort.ReadLine();
                    return result.EndsWith("\r", StringComparison.Ordinal) ? result.Substring(0, result.Length - 1) : result;
                } catch (TimeoutException) {
                    return null;
                } catch (IOException) {
                    return null;
                }
            }
        }

        #endregion


        #region Details

        public override IEnumerable<DmmMeasurementType> SupportedMeasurements {
            get {
                yield return DmmMeasurementType.VoltageAC;
                yield return DmmMeasurementType.VoltageDC;
                yield return DmmMeasurementType.Resistance;
                yield return DmmMeasurementType.Capacitance;
                yield return DmmMeasurementType.CurrentDC;
                yield return DmmMeasurementType.CurrentAC;
            }
        }

        #endregion


        #region Queries

        public override DmmIdentification GetIdentification() {
            var result = this.SendScpi("*IDN?");
            if (result == null) {
                return new DmmIdentification(null, null); //no response
            } else {
                var parts = result.Split(',');
                if (parts.Length != 4) {
                    return new DmmIdentification(null, null, null, null, result); //cannot really parse anything
                } else {
                    if (parts[3].StartsWith("V", StringComparison.Ordinal)) {
                        try {
                            return new DmmIdentification(parts[0], parts[1], parts[2], new Version(parts[3].Substring(1)), null);
                        } catch (FormatException) {
                            return new DmmIdentification(parts[0], parts[1], parts[2], null, parts[3]);
                        }
                    } else {
                        return new DmmIdentification(parts[0], parts[1], parts[2], null, parts[3]);
                    }
                }
            }
        }

        private DmmMeasurementType LastMeasurementType = null;

        public override DmmMeasurement GetCurrentMeasurement() {
            var resultRead = this.SendScpi("READ?");

            var range = GetMeasurementRange();
            var type = range.MeasurementType;

            if (!type.Equals(this.LastMeasurementType)) {
                this.LastMeasurementType = type;
                return GetCurrentMeasurement(); //repeat measurement
            } else {
                this.LastMeasurementType = type;
            }

            if (resultRead != null) {
                if (resultRead.Equals("+9.90000000E+37", StringComparison.OrdinalIgnoreCase)) {
                    return new DmmMeasurement(decimal.MaxValue, range);
                } else if (resultRead.Equals("-9.90000000E+37", StringComparison.OrdinalIgnoreCase)) {
                    return new DmmMeasurement(decimal.MinValue, range);
                } else {
                    decimal value;
                    if (decimal.TryParse(resultRead, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) {
                        return new DmmMeasurement(value, range);
                    }
                }
            }

            return null;
        }


        private static DmmMeasurementRange RangeUnknown = new DmmMeasurementRange(DmmMeasurementType.Unknown.Title, DmmMeasurementType.Unknown);
        private static DmmMeasurementRange RangeVoltageDC600m = new DmmMeasurementRange("600 mV", -3, -3, DmmMeasurementType.VoltageDC);
        private static DmmMeasurementRange RangeVoltageDC6 = new DmmMeasurementRange("6 V", 0, 0, DmmMeasurementType.VoltageDC);
        private static DmmMeasurementRange RangeVoltageDC60 = new DmmMeasurementRange("60 V", 0, 0, DmmMeasurementType.VoltageDC);
        private static DmmMeasurementRange RangeVoltageDC600 = new DmmMeasurementRange("600 V", 0, 0, DmmMeasurementType.VoltageDC);
        private static DmmMeasurementRange RangeVoltageAC600m = new DmmMeasurementRange("600 mV", -3, -3, DmmMeasurementType.VoltageAC);
        private static DmmMeasurementRange RangeVoltageAC6 = new DmmMeasurementRange("6 V", 0, 0, DmmMeasurementType.VoltageAC);
        private static DmmMeasurementRange RangeVoltageAC60 = new DmmMeasurementRange("60 V", 0, 0, DmmMeasurementType.VoltageAC);
        private static DmmMeasurementRange RangeVoltageAC600 = new DmmMeasurementRange("600 V", 0, 0, DmmMeasurementType.VoltageAC);
        private static DmmMeasurementRange RangeResistance600 = new DmmMeasurementRange("600 Ω", 0, 0, DmmMeasurementType.Resistance);
        private static DmmMeasurementRange RangeResistance6k = new DmmMeasurementRange("6 kΩ", 3, 3, DmmMeasurementType.Resistance);
        private static DmmMeasurementRange RangeResistance60k = new DmmMeasurementRange("60 kΩ", 3, 3, DmmMeasurementType.Resistance);
        private static DmmMeasurementRange RangeResistance600k = new DmmMeasurementRange("600 kΩ", 3, 3, DmmMeasurementType.Resistance);
        private static DmmMeasurementRange RangeResistance6M = new DmmMeasurementRange("6 MΩ", 6, 6, DmmMeasurementType.Resistance);
        private static DmmMeasurementRange RangeResistance60M = new DmmMeasurementRange("60 MΩ", 6, 6, DmmMeasurementType.Resistance);
        private static DmmMeasurementRange RangeDiode2 = new DmmMeasurementRange("2 V", 0, 0, DmmMeasurementType.Diode);
        private static DmmMeasurementRange RangeCapacitance1000n = new DmmMeasurementRange("1000 nF", -9, -9, DmmMeasurementType.Capacitance);
        private static DmmMeasurementRange RangeCapacitance10u = new DmmMeasurementRange("10 µF", -6, -6, DmmMeasurementType.Capacitance);
        private static DmmMeasurementRange RangeCapacitance100u = new DmmMeasurementRange("100 µF", -6, -6, DmmMeasurementType.Capacitance);
        private static DmmMeasurementRange RangeCapacitance1000u = new DmmMeasurementRange("1000 µF", -6, -6, DmmMeasurementType.Capacitance);
        private static DmmMeasurementRange RangeCapacitance10m = new DmmMeasurementRange("10 mF", -3, -3, DmmMeasurementType.Capacitance);
        private static DmmMeasurementRange RangeCurrentDC60u = new DmmMeasurementRange("60 µA", -6, -6, DmmMeasurementType.CurrentDC);
        private static DmmMeasurementRange RangeCurrentDC600u = new DmmMeasurementRange("600 µA", -6, -6, DmmMeasurementType.CurrentDC);
        private static DmmMeasurementRange RangeCurrentDC6 = new DmmMeasurementRange("6 A", 0, 0, DmmMeasurementType.CurrentDC);
        private static DmmMeasurementRange RangeCurrentDC10 = new DmmMeasurementRange("10 A", 0, 0, DmmMeasurementType.CurrentDC);
        private static DmmMeasurementRange RangeCurrentAC60u = new DmmMeasurementRange("60 µA", -6, -6, DmmMeasurementType.CurrentAC);
        private static DmmMeasurementRange RangeCurrentAC600u = new DmmMeasurementRange("600 µA", -6, -6, DmmMeasurementType.CurrentAC);
        private static DmmMeasurementRange RangeCurrentAC6 = new DmmMeasurementRange("6 A", 0, 0, DmmMeasurementType.CurrentAC);
        private static DmmMeasurementRange RangeCurrentAC10 = new DmmMeasurementRange("10 A", 0, 0, DmmMeasurementType.CurrentAC);
        private static DmmMeasurementRange RangeFrequency100 = new DmmMeasurementRange("99.99 Hz", 0, 0, DmmMeasurementType.Frequency);
        private static DmmMeasurementRange RangeFrequency1k = new DmmMeasurementRange("999.99 Hz", 0, 0, DmmMeasurementType.Frequency);
        private static DmmMeasurementRange RangeFrequency10k = new DmmMeasurementRange("9.999 kHz", 3, 3, DmmMeasurementType.Frequency);
        private static DmmMeasurementRange RangeFrequency100k = new DmmMeasurementRange("90.99 kHz", 3, 3, DmmMeasurementType.Frequency);
        private static DmmMeasurementRange RangeScale600m = new DmmMeasurementRange("600 mV", 0, 0, DmmMeasurementType.Temperature);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Contains a large switch statement.")]
        private DmmMeasurementRange GetMeasurementRange() {
            var resultConf = this.SendScpi("CONF?");

            if ((resultConf != null) && resultConf.StartsWith("\"", StringComparison.Ordinal) && resultConf.EndsWith("\"", StringComparison.Ordinal)) {
                switch (resultConf.Substring(1, resultConf.Length - 2).ToUpperInvariant()) {
                    case "V,0,AC": return RangeVoltageAC600m;
                    case "V,1,AC": return RangeVoltageAC6;
                    case "V,2,AC": return RangeVoltageAC60;
                    case "V,3,AC": return RangeVoltageAC600;
                    case "V,0,DC": return RangeVoltageDC600m;
                    case "V,1,DC": return RangeVoltageDC6;
                    case "V,2,DC": return RangeVoltageDC60;
                    case "V,3,DC": return RangeVoltageDC600;
                    case "RES,0": return RangeResistance600;
                    case "RES,1": return RangeResistance6k;
                    case "RES,2": return RangeResistance60k;
                    case "RES,3": return RangeResistance600k;
                    case "RES,4": return RangeResistance6M;
                    case "RES,5": return RangeResistance60M;
                    case "DIOD": return RangeDiode2;
                    case "CAP,0": return RangeCapacitance1000n;
                    case "CAP,1": return RangeCapacitance10u;
                    case "CAP,2": return RangeCapacitance100u;
                    case "CAP,3": return RangeCapacitance1000u;
                    case "CAP,4": return RangeCapacitance10m;
                    case "A,0,DC": return RangeCurrentDC6;
                    case "A,1,DC": return RangeCurrentDC10;
                    case "A,0,AC": return RangeCurrentAC6;
                    case "A,1,AC": return RangeCurrentAC10;
                    case "UA,0,DC": return RangeCurrentDC60u;
                    case "UA,1,DC": return RangeCurrentDC600u;
                    case "UA,0,AC": return RangeCurrentAC60u;
                    case "UA,1,AC": return RangeCurrentAC600u;
                    case "FREQ,0,AC": return RangeFrequency100;
                    case "FREQ,1,AC": return RangeFrequency1k;
                    case "FREQ,2,AC": return RangeFrequency10k;
                    case "FREQ,3,AC": return RangeFrequency100k;
                    case "MV,1,DC": return RangeScale600m;
                }
            }
            return RangeUnknown;
        }

        #endregion

    }
}
