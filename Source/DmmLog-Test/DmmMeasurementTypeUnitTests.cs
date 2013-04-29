using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DmmLogTest {

    [TestClass()]
    public class DmmMeasurementTypeUnitTests  {

        [TestMethod()]
        public void DmmMeasurementType_Keys() {
            Assert.AreEqual("", DmmMeasurementType.Unknown.Key);
            Assert.AreEqual("VoltageDC", DmmMeasurementType.VoltageDC.Key);
            Assert.AreEqual("VoltageAC", DmmMeasurementType.VoltageAC.Key);
            Assert.AreEqual("Resistance", DmmMeasurementType.Resistance.Key);
            Assert.AreEqual("Diode", DmmMeasurementType.Diode.Key);
            Assert.AreEqual("Capacitance", DmmMeasurementType.Capacitance.Key);
            Assert.AreEqual("CurrentDC", DmmMeasurementType.CurrentDC.Key);
            Assert.AreEqual("CurrentAC", DmmMeasurementType.CurrentAC.Key);
            Assert.AreEqual("Frequency", DmmMeasurementType.Frequency.Key);
            Assert.AreEqual("Temperature", DmmMeasurementType.Temperature.Key);
        }

        [TestMethod()]
        public void DmmMeasurementType_Units() {
            Assert.AreEqual("", DmmMeasurementType.Unknown.Unit);
            Assert.AreEqual("V", DmmMeasurementType.VoltageDC.Unit);
            Assert.AreEqual("V", DmmMeasurementType.VoltageAC.Unit);
            Assert.AreEqual("Ω", DmmMeasurementType.Resistance.Unit);
            Assert.AreEqual("V", DmmMeasurementType.Diode.Unit);
            Assert.AreEqual("F", DmmMeasurementType.Capacitance.Unit);
            Assert.AreEqual("A", DmmMeasurementType.CurrentDC.Unit);
            Assert.AreEqual("A", DmmMeasurementType.CurrentAC.Unit);
            Assert.AreEqual("Hz", DmmMeasurementType.Frequency.Unit);
            Assert.AreEqual("°C", DmmMeasurementType.Temperature.Unit);
        }

    }
}
