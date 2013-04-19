using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DmmLogTest {

    [TestClass()]
    public class DmmMeasurementRangeUnitTests {

        [TestMethod()]
        public void DmmMeasurementRange_Basic() {
            var r = new DmmMeasurementRange("Test", -3, 3, DmmMeasurementType.VoltageDC);
            Assert.AreEqual("Test", r.Title);
            Assert.AreEqual(-3, r.MinimumExponent);
            Assert.AreEqual(3, r.MaximumExponent);
            Assert.AreEqual(DmmMeasurementType.VoltageDC, r.MeasurementType);
        }

        [TestMethod()]
        public void DmmMeasurementRange_ExtraMin() {
            var r = new DmmMeasurementRange("Test", -100, 0, DmmMeasurementType.Unknown);
            Assert.AreEqual(-9, r.MinimumExponent);
            Assert.AreEqual(0, r.MaximumExponent);
        }

        [TestMethod()]
        public void DmmMeasurementRange_SlightlyOver() {
            var r = new DmmMeasurementRange("Test", -4, 4, DmmMeasurementType.VoltageDC);
            Assert.AreEqual("Test", r.Title);
            Assert.AreEqual(-6, r.MinimumExponent);
            Assert.AreEqual(6, r.MaximumExponent);
            Assert.AreEqual(DmmMeasurementType.VoltageDC, r.MeasurementType);
        }

        [TestMethod()]
        public void DmmMeasurementRange_SlightlyOver2() {
            var r = new DmmMeasurementRange("Test", 2, 4, DmmMeasurementType.VoltageDC);
            Assert.AreEqual("Test", r.Title);
            Assert.AreEqual(3, r.MinimumExponent);
            Assert.AreEqual(6, r.MaximumExponent);
            Assert.AreEqual(DmmMeasurementType.VoltageDC, r.MeasurementType);
        }

        [TestMethod()]
        public void DmmMeasurementRange_ExtraMax() {
            var r = new DmmMeasurementRange("Test", 0, 100, DmmMeasurementType.Unknown);
            Assert.AreEqual(0, r.MinimumExponent);
            Assert.AreEqual(9, r.MaximumExponent);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DmmMeasurementRange_MinLargerThanMax() {
            var r = new DmmMeasurementRange("Test", 1, 0, DmmMeasurementType.Unknown);
        }

        [TestMethod()]
        public void DmmMeasurementRange_UndefinedRange() {
            var r = new DmmMeasurementRange(DmmMeasurementType.VoltageDC);
            Assert.AreEqual(-9, r.MinimumExponent);
            Assert.AreEqual(9, r.MaximumExponent);
        }

    }
}
