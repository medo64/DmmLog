﻿using DmmLogDriver;
using Medo.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
            return GetNextMeasurement();
        }


        private MovingAverage Readings = new MovingAverage(1000);
        private DmmMeasurementType Type = DmmMeasurementType.Unknown;
        private static System.Random Rnd = new System.Random();

        private DmmMeasurement GetNextMeasurement() {
            var bytes = new byte[5];

            if (this.Readings.IsEmpty || (Rnd.Next(100) == 0)) {
                switch (Rnd.Next(9)) {
                    case 0: this.Type = DmmMeasurementType.VoltageDC; break;
                    case 1: this.Type = DmmMeasurementType.VoltageAC; break;
                    case 2: this.Type = DmmMeasurementType.Resistance; break;
                    case 3: this.Type = DmmMeasurementType.Diode; break;
                    case 4: this.Type = DmmMeasurementType.Capacitance; break;
                    case 5: this.Type = DmmMeasurementType.CurrentDC; break;
                    case 6: this.Type = DmmMeasurementType.CurrentAC; break;
                    case 7: this.Type = DmmMeasurementType.Frequency; break;
                    default: this.Type = DmmMeasurementType.Unknown; break;
                }
                this.Readings.Clear();
                var newValue = Rnd.Next(-9, 10) * Math.Pow(10, Rnd.Next(-8, 9));
                this.Readings.Add(newValue);
            } else {
                var currValue = this.Readings.Average;
                var newDelta = Rnd.Next(-200, 201) / 100.0;
                var newRange = (currValue != 0) ? Math.Pow(10, Math.Truncate((Math.Log10(Math.Abs(currValue))))) : 1;
                if (newRange < 1) { newRange /= 10; }
                var newValue = currValue + newDelta * newRange;
                this.Readings.Add(newValue);
            }

            return new DmmMeasurement(Convert.ToDecimal(this.Readings.Average), this.Type);
        }

        #endregion

    }
}
