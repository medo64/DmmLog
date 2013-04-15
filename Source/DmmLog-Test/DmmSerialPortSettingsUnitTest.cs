using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO.Ports;

namespace DmmLogTest {

    [TestClass()]
    public class DmmSerialPortSettingsUnitTest {

        [TestMethod()]
        public void DmmSerialPortSettings_Create() {
            var x = new DmmSerialPortSettings("COM1", 9600, Parity.None, 8, StopBits.One);
            Assert.AreEqual("COM1:9600,N,8,1", x.ToString());
        }

        [TestMethod()]
        public void DmmSerialPortSettings_CreateEmpty() {
            var x = new DmmSerialPortSettings();
            Assert.AreEqual("COM1:9600,N,8,1", x.ToString());
        }

        [TestMethod()]
        public void DmmSerialPortSettings_CreateWithColon() {
            var x = new DmmSerialPortSettings("COM2:", 9600, Parity.None, 8, StopBits.One);
            Assert.AreEqual("COM2:9600,N,8,1", x.ToString());
        }

        [TestMethod()]
        public void DmmSerialPortSettings_CreateUnusual() {
            var x = new DmmSerialPortSettings("COM 11:", 19201, Parity.Even, 7, StopBits.OnePointFive);
            Assert.AreEqual("COM11:19201,E,7,1.5", x.ToString());
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DmmSerialPortSettings_CreateInvalidPortName() {
            var x = new DmmSerialPortSettings("COMX", 9600, Parity.None, 8, StopBits.One);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DmmSerialPortSettings_CreateInvalidPortPrefix() {
            var x = new DmmSerialPortSettings("MOC1", 9600, Parity.None, 8, StopBits.One);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DmmSerialPortSettings_CreateInvalidPortPrefix2() {
            var x = new DmmSerialPortSettings("COMX1", 9600, Parity.None, 8, StopBits.One);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DmmSerialPortSettings_CreateInvalidDataBits() {
            var x = new DmmSerialPortSettings("COM1", 9600, Parity.None, 4, StopBits.One);
        }


        [TestMethod()]
        public void DmmSerialPortSettings_Parse() {
            var x = DmmSerialPortSettings.Parse("COM 3:4800,o,7,2");
            Assert.AreEqual("COM3", x.PortName);
            Assert.AreEqual(4800, x.BaudRate);
            Assert.AreEqual(Parity.Odd, x.Parity);
            Assert.AreEqual(7, x.DataBits);
            Assert.AreEqual(StopBits.Two, x.StopBits);
            Assert.AreEqual("COM3:4800,O,7,2", x.ToString());
        }

        [TestMethod()]
        public void DmmSerialPortSettings_ParseEmpty() {
            var x = DmmSerialPortSettings.Parse("");
            Assert.AreEqual("COM1:9600,N,8,1", x.ToString());
        }

        [TestMethod()]
        [ExpectedException(typeof(FormatException))]
        public void DmmSerialPortSettings_ParseNonEmpty() {
            var x = DmmSerialPortSettings.Parse(" ");
        }

        [TestMethod()]
        public void DmmSerialPortSettings_ParseNoStopBits() {
            var x = DmmSerialPortSettings.Parse("COM5:1200,e,5,0");
            Assert.AreEqual("COM5", x.PortName);
            Assert.AreEqual(1200, x.BaudRate);
            Assert.AreEqual(Parity.Even, x.Parity);
            Assert.AreEqual(5, x.DataBits);
            Assert.AreEqual(StopBits.None, x.StopBits);
            Assert.AreEqual("COM5:1200,E,5,0", x.ToString());
        }

        [TestMethod()]
        public void DmmSerialPortSettings_ParsePartial() {
            var x = DmmSerialPortSettings.Parse("COM4:");
            Assert.AreEqual("COM4:9600,N,8,1", x.ToString());
        }

        [TestMethod()]
        public void DmmSerialPortSettings_ParsePartialWithoutColon() {
            var x = DmmSerialPortSettings.Parse("COM4");
            Assert.AreEqual("COM4:9600,N,8,1", x.ToString());
        }

        [TestMethod()]
        [ExpectedException(typeof(FormatException))]
        public void DmmSerialPortSettings_ParseZeroPort() {
            var x = DmmSerialPortSettings.Parse("COM0");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormatException))]
        public void DmmSerialPortSettings_ParseNegativePort() {
            var x = DmmSerialPortSettings.Parse("COM-1:");
        }

    }
}
