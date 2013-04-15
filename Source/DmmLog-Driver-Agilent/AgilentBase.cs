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
                this.SerialPort.Write(ASCIIEncoding.ASCII.GetBytes(command + "\n"));

                try {
                    var result = this.SerialPort.ReadLine();
                    return result.EndsWith("\r", StringComparison.Ordinal) ? result.Substring(0, result.Length - 1) : result;
                } catch (TimeoutException) {
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

        public override DmmMeasurement GetCurrentMeasurement() {
            var result = this.SendScpi("READ?");

            decimal value;
            if (decimal.TryParse(result, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) {
                return new DmmMeasurement(value, DmmMeasurementType.Unknown, 1M);
            }

            return null;
        }

        #endregion

    }
}
