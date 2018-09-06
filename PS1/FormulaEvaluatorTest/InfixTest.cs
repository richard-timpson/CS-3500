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
        public static bool ValidTest(string expression, Evaluator.Lookup lookup, int expected)
        {
            Console.WriteLine(expression);
            try
            {
                return Evaluator.Evaluate(expression, lookup) == expected;
            }
            catch
            {
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
            string invalidTest = "";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void InvalidTestMethod5()
        {
            string invalidTest = "";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
        [TestMethod]
        public void InvalidTestMethod6()
        {
            string invalidTest = "";
            bool invTest = InvalidTest(invalidTest, lookup);
            Assert.IsTrue(invTest);
        }
    }
}
