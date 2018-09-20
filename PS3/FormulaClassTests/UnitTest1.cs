using System;
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
        /// Testing variables with letter followed by number and letter
        /// </summary>
        [TestMethod]
        public void VariableTest8()
        {
            string expression = "2 + a1a";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with letter followed by numbers and letters
        /// </summary>
        [TestMethod]
        public void VariableTest9()
        {
            string expression = "2 + a1a1a1za";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with letter followed by numbers and underscores
        /// </summary>
        [TestMethod]
        public void VariableTest10()
        {
            string expression = "2 + a_1_1_1_1_1_1";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with letter followed by  underscores and letters
        /// </summary>
        [TestMethod]
        public void VariableTest11()
        {
            string expression = "2 + a_a_a_a_a_a_a";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing variables with letter followed by  underscores, letters, and numbers
        /// </summary>
        [TestMethod]
        public void VariableTest12()
        {
            string expression = "2 + a_1_a_1_1_1_a";
            Formula t = new Formula(expression);
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

        /// <summary>
        /// Testing if an opening paren or operator is followed by a number, variable, or opening parenthesis.
        /// In this case testing with opening parenthesis and number
        /// </summary>
        [TestMethod]
        public void FollowCategory1Test1()
        {
            string expression = "(8 + 8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if an opening paren or operator is followed by a number, variable, or opening parenthesis.
        /// In this case testing with opening parenthesis and variable
        /// </summary>
        [TestMethod]
        public void FollowCategory1Test2()
        {
            string expression = "(a1 + 8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if an opening paren or operator is followed by a number, variable, or opening parenthesis.
        /// In this case testing with opening parenthesis and variable
        /// </summary>
        [TestMethod]
        public void FollowCategory1Test3()
        {
            string expression = "((a1 + 8))";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if an opening paren or operator is followed by a number, variable, or opening parenthesis.
        /// In this case testing with an operator and number
        /// </summary>
        [TestMethod]
        public void FollowCategory1Test4()
        {
            string expression = "(a1 + 8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if an opening paren or operator is followed by a number, variable, or opening parenthesis.
        /// In this case testing with an operator and variable
        /// </summary>
        [TestMethod]
        public void FollowCategory1Test5()
        {
            string expression = "(a1 + a1)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if an opening paren or operator is followed by a number, variable, or opening parenthesis.
        /// In this case testing with an operator and opening parenthesis
        /// </summary>
        [TestMethod]
        public void FollowCategory1Test6()
        {
            string expression = "(a1 + (1 + 1))";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if a number, variable, or closing paren is followed by operator or closing paren
        /// in this case testing with a number and an operator
        /// </summary>
        [TestMethod]
        public void FollowCategory2Test1()
        {
            string expression = "8 + 8";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if a number, variable, or closing paren is followed by operator or closing paren
        /// In this case testing with a number and closing paren
        /// </summary>
        [TestMethod]
        public void FollowCategory2Test2()
        {
            string expression = "(8) + (8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if a number, variable, or closing paren is followed by operator or closing paren
        /// In this case testing with a variable and an operator
        /// </summary>
        [TestMethod]
        public void FollowCategory2Test3()
        {
            string expression = "a1 + a1";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if a number, variable, or closing paren is followed by operator or closing paren
        /// In this case testing with a variable and a closing paren
        /// </summary>
        [TestMethod]
        public void FollowCategory2Test4()
        {
            string expression = "(a1) + (a1)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if a number, variable, or closing paren is followed by operator or closing paren
        /// In this case testing with a closing paren and an operator
        /// </summary>
        [TestMethod]
        public void FollowCategory2Test5()
        {
            string expression = "(8+9) + (8+9)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if a number, variable, or closing paren is followed by operator or closing paren
        /// In this case testing with a closing paren and a closing paren
        /// </summary>
        [TestMethod]
        public void FollowCategory2Test6()
        {
            string expression = "((8+9) + (8+9))";
            Formula t = new Formula(expression);
        }


        /// <summary>
        /// Big stress test
        /// </summary>
        [TestMethod]
        public void StressTest1()
        {
            string expression = "((((8+9 -4.34 +a123 - _45asdf / (12.567- v6))* (9-4.4 + _1) ) -2/3) * 4.5672345)";
            Formula t = new Formula(expression);
        }


        /// <summary>
        /// Simple add test
        /// </summary>
        [TestMethod]
        public void EvaluateTest1()
        {
            string expression = "2+2";
            Formula t = new Formula(expression);
            double correct_value = 4;
            object value = t.Evaluate(s => 0);
            Assert.AreEqual(correct_value, value);
        }


        /// <summary>
        /// Divide by 0 test
        /// </summary>
        [TestMethod]
        public void EvaluateTest2()
        {
            string expression = "2/(2-2)";
            Formula t = new Formula(expression);
            double correct_value = 4;
            object value = t.Evaluate(s => 0);
            Assert.AreEqual(correct_value, value);
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
        /// Testing a greater number of left parens that right parens. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenthesisTest1()
        {
            string expression = "((2+4)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing a greater number of right parens that left parens. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenthesisTest2()
        {
            string expression = "(2+4))";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Random parenthesis test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenthesisTest3()
        {
            string expression = "((((4 + 4) - 4) - 4)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if the first token in the expression is valid.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FirstTokenTest1()
        {
            string expression = "+ 5 -1";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if the last token in the expression is valid.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void LastTokenTest1()
        {
            string expression = "5 -1+";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if the last token in the expression is valid.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void LastTokenTest2()
        {
            string expression = "5 -1(";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with number
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest1()
        {
            string expression = "5 -1a";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with letter is followed by parenthesis
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest2()
        {
            string expression = "5 -a(1";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with underscore is followed by parenthesis
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest3()
        {
            string expression = "5 -_(1";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with letter is followed by operator
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest4()
        {
            string expression = "5 - a+";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with letter is followed by random character
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest5()
        {
            string expression = "5 - a*";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with letter is followed by random character in midst of correct characters
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest6()
        {
            string expression = "5 - a12345&";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with underscore is followed operator
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest7()
        {
            string expression = "5 - _-";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if starting with underscore is followed operator
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void VariableTest8()
        {
            string expression = "5 - _-";
            Formula t = new Formula(expression);
        }



        /// <summary>
        /// Testing if opening paren or operator is not followed by number variable or opening paren
        /// In this case it will be an opening parent with an operator
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory1Test1()
        {
            string expression = "(+8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if opening paren or operator is not followed by number variable or opening paren
        /// In this case it will an opening parent with a closed parenthesis
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory1Test2()
        {
            string expression = "() 8 - 8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if opening paren or operator is not followed by number variable or opening paren
        /// In this case it will be an operator with an operator
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory1Test3()
        {
            string expression = "(8 + +)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if opening paren or operator is not followed by number variable or opening paren
        /// In this case it will be an opening paren with a closed parenthesis
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory1Test4()
        {
            string expression = "(8 + ))";
            Formula t = new Formula(expression);
        }



        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a closed paren with a number
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test1()
        {
            string expression = "(8 + )9 + 9";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a closed paren with a variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test2()
        {
            string expression = "(8 + )a1 + 0";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a closed paren with an opening paren
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test3()
        {
            string expression = "(8 + )(8 + 9)";
            Formula t = new Formula(expression);
        }


        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a variable with an opening paren
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test4()
        {
            string expression = "(8 + a1(8 - 9)";
            Formula t = new Formula(expression);
        }


        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a variable with a number
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test5()
        {
            string expression = "(8 + a1 8 - 9)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a variable with a variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test6()
        {
            string expression = "(8 + a1 a1 - 9)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a number with a an opening parent
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test7()
        {
            string expression = "(8 + 8(8-8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a number with a an number
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test8()
        {
            string expression = "(8 + 8 8 - 8)";
            Formula t = new Formula(expression);
        }

        /// <summary>
        /// Testing if number, variable, or closing paren is not followed by operator or closing parenthesis. 
        /// In this case it will be a number with a variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]

        public void FollowCategory2Test9()
        {
            string expression = "(8 + 8 a1 - 8)";
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

        /// <summary>
        /// Big stress test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void StressTest1()
        {
            string expression = "((((8+9 -4.34 +a123 - _45asdf / (12.567- v6))* (9-4.4 + _1) ) -2/3) * 4.5672345))";
            Formula t = new Formula(expression);
        }




    }
}
