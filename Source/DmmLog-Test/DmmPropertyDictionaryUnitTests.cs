using DmmLogDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace DmmLogTest {

    [TestClass()]
    public class DmmPropertyDictionaryUnitTests {

        [TestMethod()]
        public void DmmPropertyDictionary_Basic() {
            var props = new DmmPropertyDictionary();
            props.Add("Item1", "Value1");
            props.Add("Item2", "Value2");
            Assert.AreEqual(2, props.Count);
            Assert.AreEqual("Value1", props["Item1"]);
            Assert.AreEqual("Value2", props["Item2"]);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DmmPropertyDictionary_ReadOnly1() {
            var props = new DmmPropertyDictionary();
            props.Add("Item1", "Value1");
            props.Add("Item2", "Value2");
            props = props.AsReadOnly();
            props.Clear();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DmmPropertyDictionary_ReadOnly2() {
            var props = new DmmPropertyDictionary();
            props.Add("Item1", "Value1");
            props.Add("Item2", "Value2");
            props = props.AsReadOnly();
            props.Remove("Item1");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DmmPropertyDictionary_ReadOnly3() {
            var props = new DmmPropertyDictionary();
            props.Add("Item1", "Value1");
            props.Add("Item2", "Value2");
            props = props.AsReadOnly();
            props.Add("Item3", "Value3");
        }

    }
}
