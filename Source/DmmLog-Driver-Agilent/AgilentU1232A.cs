using DmmLogDriver;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DmmLogDriverAgilent {
    [Category("Agilent")]
    [DisplayName("U1232A")]
    [Description("Serial")]
    public class AgilentU1232A : AgilentBase {

        public AgilentU1232A(String settings)
            : base(settings) {
        }


        #region Communication

        public override void Connect() {
            base.Connect();
            if (base.IsConnected) {
                var id = this.GetIdentification();
                if (!("U1232A".Equals(id.Model))) { base.Disconnect(); }
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

    }
}
