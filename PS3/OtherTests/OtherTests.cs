using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Linq;
using System.Collections.Generic;

namespace OtherTests
{
    [TestClass]
    public class OtherTests
    {
        /// <summary>
        /// Testing if it correctly skips over a duplicate
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            Formula t = new Formula("a1 + a2 + a2 + a4");

            IEnumerator<string> variables = t.GetVariables().GetEnumerator();

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "a1");

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "a2");

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "a4");

            Assert.IsFalse(variables.MoveNext());
        }
        [TestMethod()]
        public void TestGetvariables1()
        {
            Formula t = new Formula("x + X + z", s => s.ToUpper(), s => true);

            IEnumerator<string> variables = t.GetVariables().GetEnumerator();

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "X");

            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual(variables.Current, "Z");

            Assert.IsFalse(variables.MoveNext());
        }
    }
}
