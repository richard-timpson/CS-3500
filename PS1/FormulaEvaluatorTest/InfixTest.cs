using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaEvaluator;
using PS1_Console_App;
namespace FormulaEvaluatorTest
{
    [TestClass]
    public class InfixFormulaTest
    {
        public static int lookupFunction(string input)
        {
            if (input == "A5")
            {
                return 10;
            }
            else if (input == "A6")
            {
                return 2;
            }
            else
            {
                throw new ArgumentException("Invalid variable");
            }
        }
        Evaluator.Lookup lookup = lookupFunction;
        public static bool ValidTest(string expression, Evaluator.Lookup lookup, int expected, out int value)
        {
            Console.WriteLine(expression);
            try
            {
                value = Evaluator.Evaluate(expression, lookup);
                return true;
            }
            catch (Exception e)
            {
                value = 0;
                Console.WriteLine(e);
                return false;
            }

        }
        public static bool InvalidTest(string expression, Evaluator.Lookup lookup)
        {
            Console.WriteLine(expression);
            try
            {
                Evaluator.Evaluate(expression, lookup);
                return false;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
        /// <summary>
        /// Testing only whitespace
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod1()
        {
            string invalidTest = "";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// Only parenthesis
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod2()
        {
            string invalidTest = "()";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// Invalid Parenthesis
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod3()
        {
            string invalidTest = "(8 * (4+2))) - 4";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// Division by 0
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod4()
        {
            string invalidTest = "5 / 0";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// Two operators in a row
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod5()
        {
            string invalidTest = " 5 ++ 5";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// Two operands in a row
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod6()
        {
            string invalidTest = "5 5 + 5";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// less than 2 operands for an operator
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod7()
        {
            string invalidTest = "4 +";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// Invalid variable name
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod8()
        {
            string invalidTest = "a&";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// Invalid formula followed by valid formula
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod9()
        {
            string invalidTest = "(8+9) -(5/2) * A6 / (5 - 5)";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
            string validTest = "(2 + 2)";
            int expected = 4;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }

        // Valid Tests

            //Basic operations


        /// <summary>
        /// add 
        /// </summary>
        [TestMethod]
        public void ValidTestMethod1()
        {
            string validTest = "2 + 2";
            int expected = 4;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  subtract
        /// </summary>
        [TestMethod]
        public void ValidTestMethod2()
        {
            string validTest = "3 - 1";
            int expected = 2;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  Subtract to get negative number
        /// </summary>
        [TestMethod]
        public void ValidTestMethod3()
        {
            string validTest = "1 - 3";
            int expected = -2;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  Multiplication 
        /// </summary>
        [TestMethod]
        public void ValidTestMethod4()
        {
            string validTest = "4 * 8";
            int expected = 32;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  Division
        /// </summary>
        [TestMethod]
        public void ValidTestMethod5()
        {
            string validTest = "8 / 4";
            int expected = 2;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  Fraction division
        /// </summary>
        [TestMethod]
        public void ValidTestMethod6()
        {
            string validTest = "4 / 8";
            int expected = 0;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }


        /// OOO

        /// <summary>
        ///  Multiplication with addition
        /// </summary>
        [TestMethod]
        public void ValidTestMethod7()
        {
            string validTest = "7 * 8 + 4";
            int expected = 60;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  Division with addition
        /// </summary>
        [TestMethod]
        public void ValidTestMethod8()
        {
            string validTest = "10/2 + 4";
            int expected = 9;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  Addition with subtraction
        /// </summary>
        [TestMethod]
        public void ValidTestMethod9()
        {
            string validTest = "4 - 4 + 10";
            int expected = 10;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  testing with parenthesis
        /// </summary>
        [TestMethod]
        public void ValidTestMethod10()
        {
            string validTest = "(7 + 4) * 8";
            int expected = 88;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  variable
        /// </summary>
        [TestMethod]
        public void ValidTestMethod11()
        {
            string validTest = "A5 + 2";
            int expected = 12;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  divide by 0 in numerator
        /// </summary>
        [TestMethod]
        public void ValidTestMethod12()
        {
            string validTest = "0 / 5";
            int expected = 0;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///  Complicated string with two variables
        /// </summary>
        [TestMethod]
        public void ValidTestMethod13()
        {
            string validTest = "(2+1) - (4*5) /(9-7) - A5 / A6 + 5";
            int expected = -7;
            int actual = 0;
            bool valTest = ValidTest(validTest, lookup, expected, out actual);
            Assert.IsTrue(valTest);
            Assert.AreEqual(expected, actual);
        }
    }
}
