using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DmmLogTest {

    [TestClass()]
    public class DmmMeasurementUnitTests {

        [TestMethod()]
        public void DmmMeasurement_Exp_0() {
            var m = new DmmMeasurement(3.14159M);
            Assert.AreEqual(3.14159M, m.Value.Coefficient);
            Assert.AreEqual(0, m.Value.Exponent);
            Assert.AreEqual("", m.Value.SIPrefix);
            Assert.AreEqual("", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M3() {
            var m = new DmmMeasurement(0.314159M, DmmMeasurementType.VoltageDC);
            Assert.AreEqual(314.159M, m.Value.Coefficient);
            Assert.AreEqual(-3, m.Value.Exponent);
            Assert.AreEqual("m", m.Value.SIPrefix);
            Assert.AreEqual("mV", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P3() {
            var m = new DmmMeasurement(3141.59M, DmmMeasurementType.VoltageAC);
            Assert.AreEqual(3.14159M, m.Value.Coefficient);
            Assert.AreEqual(3, m.Value.Exponent);
            Assert.AreEqual("k", m.Value.SIPrefix);
            Assert.AreEqual("kV~", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M6() {
            var m = new DmmMeasurement(0.0000314159M, DmmMeasurementType.Resistance);
            Assert.AreEqual(31.4159M, m.Value.Coefficient);
            Assert.AreEqual(-6, m.Value.Exponent);
            Assert.AreEqual("µ", m.Value.SIPrefix);
            Assert.AreEqual("µΩ", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P6() {
            var m = new DmmMeasurement(3141592M, DmmMeasurementType.Frequency);
            Assert.AreEqual(3.141592M, m.Value.Coefficient);
            Assert.AreEqual(6, m.Value.Exponent);
            Assert.AreEqual("M", m.Value.SIPrefix);
            Assert.AreEqual("MHz", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M9() {
            var m = new DmmMeasurement(0.000000314159M, DmmMeasurementType.CurrentDC);
            Assert.AreEqual(314.159M, m.Value.Coefficient);
            Assert.AreEqual(-9, m.Value.Exponent);
            Assert.AreEqual("n", m.Value.SIPrefix);
            Assert.AreEqual("nA", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P9() {
            var m = new DmmMeasurement(314159265358M, DmmMeasurementType.CurrentAC);
            Assert.AreEqual(314.159265358M, m.Value.Coefficient);
            Assert.AreEqual(9, m.Value.Exponent);
            Assert.AreEqual("G", m.Value.SIPrefix);
            Assert.AreEqual("GA~", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M9_Extra() {
            var m = new DmmMeasurement(0.000000000314159M, DmmMeasurementType.Capacitance);
            Assert.AreEqual(0.314159M, m.Value.Coefficient);
            Assert.AreEqual(-9, m.Value.Exponent);
            Assert.AreEqual("n", m.Value.SIPrefix);
            Assert.AreEqual("nF", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P9_Extra() {
            var m = new DmmMeasurement(314159265358979M, DmmMeasurementType.Diode);
            Assert.AreEqual(314159.265358979M, m.Value.Coefficient);
            Assert.AreEqual(9, m.Value.Exponent);
            Assert.AreEqual("G", m.Value.SIPrefix);
            Assert.AreEqual("GV", m.SIUnit);
        }


        [TestMethod()]
        public void DmmMeasurement_RangeMilliToNone_Above() {
            var r = new DmmMeasurementRange("Current", -3, 0, DmmMeasurementType.CurrentDC);
            var m = new DmmMeasurement(1000, r);
            Assert.AreEqual(1000, m.Value.Coefficient);
            Assert.AreEqual(0, m.Value.Exponent);
            Assert.AreEqual("", m.Value.SIPrefix);
            Assert.AreEqual("A", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_RangeMilliToNone_Below() {
            var r = new DmmMeasurementRange("Current", -3, 3, DmmMeasurementType.CurrentDC);
            var m = new DmmMeasurement(0.0001M, r);
            Assert.AreEqual(0.1M, m.Value.Coefficient);
            Assert.AreEqual(-3, m.Value.Exponent);
            Assert.AreEqual("m", m.Value.SIPrefix);
            Assert.AreEqual("mA", m.SIUnit);
        }

    }
}
