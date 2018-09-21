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
        [TestMethod()]
        public void TestToString()
        {
            Formula t = new Formula("x+y");

            string s = t.ToString();

            Assert.AreEqual("x+y", s);
        }
        [TestMethod()]
        public void TestToString1()
        {
            Formula t = new Formula("x+y", a=> a.ToUpper(), a=> true);

            string s = t.ToString();

            Assert.AreEqual("X+Y", s);
        }
        [TestMethod()]
        public void TestEquals()
        {
            Formula t = new Formula("x+y");
            Formula r = new Formula("x+y");

            Assert.IsTrue(t.Equals(r));
            
        }
        [TestMethod()]
        public void TestEquals1()
        {
            Formula t = new Formula("X+Y");
            Formula r = new Formula("x+y", s=> s.ToUpper(), s=> true);

            Assert.IsTrue(t.Equals(r));

        }
        [TestMethod()]
        public void TestEquals2()
        {
            Formula t = new Formula("x+y");
            Formula r = new Formula("X+y");

            Assert.IsFalse(t.Equals(r));
        }
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        [TestMethod()]
        public void TestEquals3()
        {
            Formula t = new Formula("2.0 + x7");
            Formula r = new Formula("2.0000 + x7");

            Assert.IsTrue(t.Equals(r));
        }
        [TestMethod()]
        public void TestEquals4()
        {
            Formula t = new Formula("2.0 + x7");
            object r = new object();

            Assert.IsFalse(t.Equals(r));
        }
        [TestMethod()]
        public void TestEquals5()
        {
            Formula t = new Formula("10e1 + x7");
            Formula r = new Formula("100 + x7");
            Assert.IsTrue(t.Equals(r));
        }

        // 1.342e20
        // 1.342 * 10^20

        [TestMethod()]
        public void TestOperatorEquals()
        {
            Formula t = new Formula("x+y");
            Formula r = new Formula("x+y");

            Assert.IsTrue(t == r);

            Assert.IsFalse(t != r);
        }
        [TestMethod()]
        public void TestOperatorEquals1()
        {
            Formula t = null;
            Formula r = null;

            Assert.IsTrue(t == r);

            Assert.IsFalse(t != r);
        }
        [TestMethod()]
        public void TestOperatorEquals2()
        {
            Formula t = null;
            Formula r = new Formula("x+y");

            Assert.IsFalse(t == r);
        }


        [TestMethod()]
        public void TestOperatorNotEquals()
        {
            Formula t = new Formula("x+Y");
            Formula r = new Formula("x+y");

            Assert.IsTrue(t != r);

            Assert.IsFalse(t == r);

        }
        [TestMethod()]
        public void HashCode1()
        {
            Formula t = new Formula("2+2");
            Formula r = new Formula("2+2");

            int hash1 = t.GetHashCode();
            int hash2 = r.GetHashCode();

            Assert.AreEqual(hash1, hash2);
        }
        [TestMethod()]
        public void HashCode2()
        {
            Formula t = new Formula("2+2");
            Formula r = new Formula("2+3");

            int hash1 = t.GetHashCode();
            int hash2 = r.GetHashCode();

            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
