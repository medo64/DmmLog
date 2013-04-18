using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DmmLogTest {

    [TestClass()]
    public class DmmMeasurementRangeUnitTests {

        [TestMethod()]
        public void DmmMeasurementRange_MilliToKilo() {
            var r = new DmmMeasurementRange("Test", 0.001M, 1000M, DmmMeasurementType.VoltageDC);
            Assert.AreEqual("Test", r.Title);
            Assert.AreEqual(0.001M, r.MinimumValue);
            Assert.AreEqual(-3, r.MinimumExponent);
            Assert.AreEqual(1000, r.MaximumValue);
            Assert.AreEqual(3, r.MaximumExponent);
            Assert.AreEqual(DmmMeasurementType.VoltageDC, r.MeasurementType);
        }

        [TestMethod()]
        public void DmmMeasurementRange_MilliToNone() {
            var r = new DmmMeasurementRange("Test", 0.001M, 999, DmmMeasurementType.Unknown);
            Assert.AreEqual(-3, r.MinimumExponent);
            Assert.AreEqual(0, r.MaximumExponent);
        }

        [TestMethod()]
        public void DmmMeasurementRange_MicroToMega() {
            var r = new DmmMeasurementRange("Test", 0.00001M, 6000000, DmmMeasurementType.Unknown);
            Assert.AreEqual(-6, r.MinimumExponent);
            Assert.AreEqual(6, r.MaximumExponent);
        }

        [TestMethod()]
        public void DmmMeasurementRange_MicroToMega10() {
            var r = new DmmMeasurementRange("Test", 0.00001M, 60000000, DmmMeasurementType.Unknown);
            Assert.AreEqual(-6, r.MinimumExponent);
            Assert.AreEqual(6, r.MaximumExponent);
        }

        [TestMethod()]
        public void DmmMeasurementRange_MicroToMega100() {
            var r = new DmmMeasurementRange("Test", 0.00001M, 600000000, DmmMeasurementType.Unknown);
            Assert.AreEqual(-6, r.MinimumExponent);
            Assert.AreEqual(6, r.MaximumExponent);
        }

        [TestMethod()]
        public void DmmMeasurementRange_NanoToGiga() {
            var r = new DmmMeasurementRange("Test", 0.0000001M, 6000000000, DmmMeasurementType.Unknown);
            Assert.AreEqual(-9, r.MinimumExponent);
            Assert.AreEqual(9, r.MaximumExponent);
        }

        [TestMethod()]
        public void DmmMeasurementRange_NanoToGigaExtra() {
            var r = new DmmMeasurementRange("Test", 0.000000000001M, 6000000000000, DmmMeasurementType.Unknown);
            Assert.AreEqual(-9, r.MinimumExponent);
            Assert.AreEqual(9, r.MaximumExponent);
        }

        [TestMethod()]
        public void DmmMeasurementRange_UndefinedRange() {
            var r = new DmmMeasurementRange("Test", DmmMeasurementType.VoltageDC);
            Assert.AreEqual(-9, r.MinimumExponent);
            Assert.AreEqual(9, r.MaximumExponent);
        }

    }
}
