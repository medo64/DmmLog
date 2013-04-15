using DmmLogDriver;
using Medo.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;

namespace DmmLogDriverAgilent {
    [DisplayName("Random")]
    [Description("")]
    public class Random : DmmDriver {

        public Random(String settings)
            : base() {
        }


        #region Communication

        private bool IsInConnectedState;

        public override void Connect() {
            if (this.IsInConnectedState) { throw new InvalidOperationException("Cannot connect: already connected."); }
            this.IsInConnectedState = true;
        }

        public override void Disconnect() {
            this.IsInConnectedState = false;
        }

        public override Boolean IsConnected {
            get { return this.IsInConnectedState; }
        }

        #endregion


        #region Details

        public override IEnumerable<DmmMeasurementType> SupportedMeasurements {
            get {
                yield return DmmMeasurementType.Unknown;
            }
        }

        #endregion


        #region Queries

        public override DmmIdentification GetIdentification() {
            return new DmmIdentification(null, "Random");
        }

        public override DmmMeasurement GetCurrentMeasurement() {
            return new DmmMeasurement(GetNextValue());
        }


        private MovingAverage Readings = new MovingAverage(1000);
        private static RandomNumberGenerator Rnd = RandomNumberGenerator.Create();

        private decimal GetNextValue() {
            var bytes = new byte[5];
            Random.Rnd.GetBytes(bytes);

            var value = BitConverter.ToInt32(bytes, 0) % 26 + 5; //-20 to +30

            if ((this.Readings.IsEmpty) || (bytes[4] == 0)) {
                this.Readings.Clear();
                for (int i = 0; i < 16; i++) {//to avoid huge changes after next number is added in
                    this.Readings.Add(value);
                }
            }
            this.Readings.Add(value);

            return Convert.ToDecimal(this.Readings.Average);
        }

        #endregion

    }
}
