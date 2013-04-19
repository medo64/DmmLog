using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace DmmLogTest {

    [TestClass()]
    public class DmmEngineeringNotationUnitTests {

        [TestMethod()]
        public void DmmEngineeringNotation_Base() {
            var x = new DmmEngineeringNotation(10);
            Assert.AreEqual(10, x.Value);
            Assert.AreEqual(10, x.Coefficient);
            Assert.AreEqual(0, x.Exponent);
            Assert.IsTrue(x == 10);
            Assert.IsTrue(x == 10M);
            Assert.IsTrue(x == 10.0);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_Milli() {
            var x = new DmmEngineeringNotation(0.1M);
            Assert.AreEqual(0.1M, x.Value);
            Assert.AreEqual(100, x.Coefficient);
            Assert.AreEqual(-3, x.Exponent);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_Micro() {
            var x = new DmmEngineeringNotation(0.000314M);
            Assert.AreEqual(0.000314M, x.Value);
            Assert.AreEqual(314, x.Coefficient);
            Assert.AreEqual(-6, x.Exponent);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_Nano() {
            var x = new DmmEngineeringNotation(0.0000000314M);
            Assert.AreEqual(0.0000000314M, x.Value);
            Assert.AreEqual(31.4M, x.Coefficient);
            Assert.AreEqual(-9, x.Exponent);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_LessThanNano() {
            var x = new DmmEngineeringNotation(0.000000000314M);
            Assert.AreEqual(0.000000000314M, x.Value);
            Assert.AreEqual(0.314M, x.Coefficient);
            Assert.AreEqual(-9, x.Exponent);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_Kilo() {
            var x = new DmmEngineeringNotation(4242);
            Assert.AreEqual(4242, x.Value);
            Assert.AreEqual(4.242M, x.Coefficient);
            Assert.AreEqual(3, x.Exponent);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_Mega() {
            var x = new DmmEngineeringNotation(3141592.65358979323846264M);
            Assert.AreEqual(3141592.65358979323846264M, x.Value);
            Assert.AreEqual(3.14159265358979323846264M, x.Coefficient);
            Assert.AreEqual(6, x.Exponent);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_Giga() {
            var x = new DmmEngineeringNotation(31415926535.8979323846264M);
            Assert.AreEqual(31415926535.8979323846264M, x.Value);
            Assert.AreEqual(31.4159265358979323846264M, x.Coefficient);
            Assert.AreEqual(9, x.Exponent);
        }

        [TestMethod()]
        public void DmmEngineeringNotation_MoreThanGiga() {
            var x = new DmmEngineeringNotation(31415926535897.9323846264M);
            Assert.AreEqual(31415926535897.9323846264M, x.Value);
            Assert.AreEqual(31415.9265358979323846264M, x.Coefficient);
            Assert.AreEqual(9, x.Exponent);
        }


    }
}
