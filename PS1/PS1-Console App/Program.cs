using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;


namespace PS1_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            Evaluator.Lookup lookup = lookupFunction; 
            //invalid tests
            string invalid_test1 = ""; // only whitespace: Successful
            string invalid_test2 = "()"; // only parenthesis: Successful
            string invalid_test3 = "(8 * (4+2))) - 4"; //Invalid parenthesis: 
            string invalid_test4 = " 5 / 0"; //division by 0 
            string invalid_test5 = "5 ++5 "; //two operators in a row
            string invalid_test6 = "5 5 + 5"; //two operands in a row"
            string invalid_test7 = "4 +"; // <2 operands for an operator
            string invalid_test8 = "a&"; // invalid variable name
            string invalid_test9 = "(8+9) -(5/2) * A6 / (5 - 5)"; //invalid formula
            string invalid_test10 = "(2 + 2)"; // followed by valid formula

            //valid tests
            
            //basic operations
            string valid_test1 = "2 + 2"; // add 4
            string valid_test2 = "3 -1 "; //subtract 2
            string valid_test3 = "1 - 3"; //subtract to get negative number -2
            string valid_test4 = "4 * 8"; //multiplication 32
            string valid_test5 = "8/4"; //division 2
            string valid_test6 = "4 / 8"; //fraction .5

            //order of operations

            string valid_test7 = "7 * 8 + 4"; // multiplication with addition
            string valid_test8 = "10/2 + 4"; // division with addition
            string valid_test9 = "4 - 4 + 10"; //addition and subtraction
            string valid_test10 = "(7 + 4) * 8"; //testing with simple parenthesis


            string valid_test11 = "(2+1) - (4*5) /(9-7) - A5 / A6 + 5"; // random string
            string valid_test12 = "0 / 5"; //divide by 0 in numerator

            bool invTest = InvalidTest(invalid_test3, lookup);
            if (invTest)
            {
                Console.WriteLine("Test was invalid");
            }
            int expected = 0;
            bool validTest = ValidTest(valid_test12, lookup, expected);
            if (validTest)
            {
                Console.WriteLine("Test was Valid. Output: " +  expected );
            }

            Console.Read();

        }
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
        public static bool ValidTest(string expression, Evaluator.Lookup lookup , int expected) 
        {
            Console.WriteLine(expression);
            try {
               return Evaluator.Evaluate(expression, lookup) == expected;
            }
            catch {
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
            catch(Exception e) 
            {
                Console.WriteLine(e);
                return false;
            }

        }
    }
}
