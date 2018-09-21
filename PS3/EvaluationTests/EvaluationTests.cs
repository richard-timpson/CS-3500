using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
namespace EvaluationTests
{
    [TestClass]
    public class ValidValues
    {

        [TestMethod()]
        public void TestSingleNumber()
        {
            Formula t = new Formula("5");
            double value = 5;
            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestSingleVariable()
        {
            Formula t = new Formula("X5");
            double value = 13;

            Assert.AreEqual(value, t.Evaluate(s => 13));
        }

        [TestMethod()]
        public void TestAddition()
        {
            Formula t = new Formula("5+3");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate( s => 0));
        }

        [TestMethod()]
        public void TestSubtraction()
        {
            Formula t = new Formula("18-10");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestMultiplication()
        {
            Formula t = new Formula("2*4");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestDivision()
        {
            Formula t = new Formula("16/2");
            double value = 8;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestArithmeticWithVariable()
        {
            Formula t = new Formula("2+X1");
            double value = 6;

            Assert.AreEqual(value, t.Evaluate(s => 4));
        }
        [TestMethod()]
        public void TestLeftToRight()
        {
            Formula t = new Formula("2*6+3");
            double value = 15;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestOrderOperations()
        {
            Formula t = new Formula("2+6*3");
            double value = 20;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestParenthesesTimes()
        {
            Formula t = new Formula("(2+6)*3");
            double value = 24;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestTimesParentheses()
        {
            Formula t = new Formula("2*(3+5)");
            double value = 16;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestPlusParentheses()
        {
            Formula t = new Formula("2+(3+5)");
            double value = 10;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestPlusComplex()
        {
            Formula t = new Formula("2+(3+5*9)");
            double value = 50;

            Assert.AreEqual(value, t.Evaluate( s => 0));
        }

        [TestMethod()]
        public void TestComplexTimesParentheses()
        {
            Formula t = new Formula("2+3*(3+5)");
            double value = 26;

            Assert.AreEqual(value, t.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestComplexAndParentheses()
        {
            Formula t = new Formula("2+3*5+(3+4*8)*5+2");
            double value = 194;

            Assert.AreEqual(value, t.Evaluate( s => 0));
        }
        [TestMethod()]
        public void TestComplexMultiVar()
        {
            Formula t = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            double value = 6;

            //commented out because the expected value is an integer division, and not
            // a double division. 

            //Assert.AreEqual(value, t.Evaluate(s => (s == "x7") ? 1 : 4));
        }

        [TestMethod()]
        public void TestComplexNestedParensRight()
        {
            Formula t = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            double value = 6;

            Assert.AreEqual(value, t.Evaluate(s => 1));
        }

        [TestMethod()]
        public void TestComplexNestedParensLeft()
        {
            Formula t = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            double value = 12;

            Assert.AreEqual(value, t.Evaluate( s => 2));
        }

        [TestMethod()]
        public void TestRepeatedVar()
        {
            Formula t = new Formula("a4-a4*a4/a4");
            double value = 0;

            Assert.AreEqual(value, t.Evaluate(s => 3));
        }

    }

    /// <summary>
    ///This is a test class for EvaluatorTest and is intended
    ///to contain all EvaluatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvalidValues
    {

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUnknownVariable()
        {
            Formula t = new Formula("2+X1");

            t.Evaluate( s => { throw new ArgumentException("Unknown variable"); });
        }


        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDivideByZero()
        {
            Formula t = new Formula("5/0");

            t.Evaluate( s => 0);
        }

        /// <summary>
        /// Testing with computed value in the division
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDivideByZero1()
        {
            Formula t = new Formula("5/(5-5)");

            t.Evaluate(s => 0);
        }
    }
}
