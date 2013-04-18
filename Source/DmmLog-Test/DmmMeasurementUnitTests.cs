using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DmmLogTest {

    [TestClass()]
    public class DmmMeasurementUnitTests {

        [TestMethod()]
        public void DmmMeasurement_Exp_0() {
            var m = new DmmMeasurement(3.14159M);
            Assert.AreEqual(3.14159M, m.EngineeringCoefficient);
            Assert.AreEqual(0, m.EngineeringExponent);
            Assert.AreEqual("", m.SIPrefix);
            Assert.AreEqual("", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M3() {
            var m = new DmmMeasurement(0.314159M, DmmMeasurementType.VoltageDC);
            Assert.AreEqual(314.159M, m.EngineeringCoefficient);
            Assert.AreEqual(-3, m.EngineeringExponent);
            Assert.AreEqual("m", m.SIPrefix);
            Assert.AreEqual("mV", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P3() {
            var m = new DmmMeasurement(3141.59M, DmmMeasurementType.VoltageAC);
            Assert.AreEqual(3.14159M, m.EngineeringCoefficient);
            Assert.AreEqual(3, m.EngineeringExponent);
            Assert.AreEqual("k", m.SIPrefix);
            Assert.AreEqual("kV~", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M6() {
            var m = new DmmMeasurement(0.0000314159M, DmmMeasurementType.Resistance);
            Assert.AreEqual(31.4159M, m.EngineeringCoefficient);
            Assert.AreEqual(-6, m.EngineeringExponent);
            Assert.AreEqual("μ", m.SIPrefix);
            Assert.AreEqual("μΩ", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P6() {
            var m = new DmmMeasurement(3141592M, DmmMeasurementType.Frequency);
            Assert.AreEqual(3.141592M, m.EngineeringCoefficient);
            Assert.AreEqual(6, m.EngineeringExponent);
            Assert.AreEqual("M", m.SIPrefix);
            Assert.AreEqual("MHz", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M9() {
            var m = new DmmMeasurement(0.000000314159M, DmmMeasurementType.CurrentDC);
            Assert.AreEqual(314.159M, m.EngineeringCoefficient);
            Assert.AreEqual(-9, m.EngineeringExponent);
            Assert.AreEqual("n", m.SIPrefix);
            Assert.AreEqual("nA", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P9() {
            var m = new DmmMeasurement(314159265358M, DmmMeasurementType.CurrentAC);
            Assert.AreEqual(314.159265358M, m.EngineeringCoefficient);
            Assert.AreEqual(9, m.EngineeringExponent);
            Assert.AreEqual("G", m.SIPrefix);
            Assert.AreEqual("GA~", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_M9_Extra() {
            var m = new DmmMeasurement(0.000000000314159M, DmmMeasurementType.Capacitance);
            Assert.AreEqual(0.314159M, m.EngineeringCoefficient);
            Assert.AreEqual(-9, m.EngineeringExponent);
            Assert.AreEqual("n", m.SIPrefix);
            Assert.AreEqual("nF", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_Exp_P9_Extra() {
            var m = new DmmMeasurement(314159265358979M, DmmMeasurementType.Diode);
            Assert.AreEqual(314159.265358979M, m.EngineeringCoefficient);
            Assert.AreEqual(9, m.EngineeringExponent);
            Assert.AreEqual("G", m.SIPrefix);
            Assert.AreEqual("GV", m.SIUnit);
        }


        [TestMethod()]
        public void DmmMeasurement_RangeMilliToNone_Above() {
            var r = new DmmMeasurementRange("Current", 0.001M, 20M, DmmMeasurementType.CurrentDC);
            var m = new DmmMeasurement(1000, r);
            Assert.AreEqual(1000, m.EngineeringCoefficient);
            Assert.AreEqual(0, m.EngineeringExponent);
            Assert.AreEqual("", m.SIPrefix);
            Assert.AreEqual("A", m.SIUnit);
        }

        [TestMethod()]
        public void DmmMeasurement_RangeMilliToNone_Below() {
            var r = new DmmMeasurementRange("Current", 0.001M, 20M, DmmMeasurementType.CurrentDC);
            var m = new DmmMeasurement(0.0001M, r);
            Assert.AreEqual(0.1M, m.EngineeringCoefficient);
            Assert.AreEqual(-3, m.EngineeringExponent);
            Assert.AreEqual("m", m.SIPrefix);
            Assert.AreEqual("mA", m.SIUnit);
        }

    }
}
