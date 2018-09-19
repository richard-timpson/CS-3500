﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaClassTests
{
    [TestClass]
    public class ValidExpressions
    {
        /// <summary>
        /// Testing simple integer with addition
        /// </summary>
        [TestMethod]
        public void AdditionTest()
        {
            string expression = "5 + 5";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing integers with subtraction
        /// </summary>
        [TestMethod]
        public void SubtractionTest()
        {
            string expression = "5 - 5";
            Formula t = new Formula(expression);
        }
        /// <summary>
        /// Testing integers with multiplication
        /// </summary>
        [TestMethod]
        public void MultiplicationTest()
        {
            string expression = "5 * 5";
            Formula t = new Formula(expression);
        }
        /// <summary>
        /// Testing integers with division
        /// </summary>
        [TestMethod]
        public void DivisionTest()
        {
            string expression = "5 / 5";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with letter and number
        /// </summary>
        [TestMethod]
        public void VariableTest1()
        {
            string expression = "a1 + a1";
            Formula t = new Formula(expression);

            expression = "a1111 + a1111";

            Formula s = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with single letter
        /// </summary>
        [TestMethod]
        public void VariableTest2()
        {
            string expression = "a + a";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with letters followed by underscore
        /// </summary>
        [TestMethod]
        public void VariableTest3()
        {
            string expression = "a_ + a_";
            Formula t = new Formula(expression);

            expression = "a___ + a___";
            Formula s = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with letters followed by letters
        /// </summary>
        [TestMethod]
        public void VariableTest4()
        {
            string expression = "aa + aa";
            Formula t = new Formula(expression);

            expression = "aaa + aaa";
            Formula s = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with just underscores
        /// </summary>
        [TestMethod]
        public void VariableTest5()
        {
            string expression = "_ + _";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with underscores followed by letters
        /// </summary>
        [TestMethod]
        public void VariableTest6()
        {
            string expression = "_a + _a";
            Formula t = new Formula(expression);

            expression = "_aaaa + _aaaa";
            Formula s = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with underscores followed by number
        /// </summary>
        [TestMethod]
        public void VariableTest7()
        {
            string expression = "_1 + _1";
            Formula t = new Formula(expression);

            expression = "_11111 + _111111";
            Formula s = new Formula(expression);
        }


        /// <summary>
        /// Testing variables and integers with addition
        /// </summary>
        [TestMethod]
        public void VariableIntegerTest()
        {
            string expression = "5 + a1";
            Formula t = new Formula(expression);
        }
        /// <summary>
        /// testing simple double
        /// </summary>
        [TestMethod]
        public void DoubleTest1()
        {
            string expression = "2.1 + 2.1";
            Formula t = new Formula(expression);
        }
        /// <summary>
        /// testing very long doubles
        /// </summary>
        [TestMethod]
        public void DoubleTest2()
        {
            string expression = "2.1111111100000002222 + 2.1111111110000000222222";
            Formula t = new Formula(expression);
        }

    }
    [TestClass]
    public class InvalidExpressions
    {
        /// <summary>
        /// Testing if there is at least one token
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyTest()
        {
            string expression = " ";
            Formula t = new Formula(expression);
        }
        /// <summary>
        /// Testing a non-valid token
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TokenTest1()
        {
            string expression = "@ + 1";
            Formula t = new Formula(expression);
        }
        /// <summary>
        /// Testing a non-valid variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void VariableTest1()
        {
            string expression = "2 + a1a";
            Formula t = new Formula(expression);
        }






        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidExpressionTest1()
        {
            string expression = "5 + ";
            Formula t = new Formula(expression);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidExpressionTest2()
        {
            string expression = "(5 + 5";
            Formula t = new Formula(expression);
        }




    }
}
