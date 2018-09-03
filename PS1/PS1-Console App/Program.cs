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
            string test = "(2+1) - (4*5) /(9-7) - A5 / A6 + 5";
            //string test = "(1+1) -(4*5)/(9-7)";
            Evaluator.Lookup evaluate = lookupFunction;
            int value = Evaluator.Evaluate(test, evaluate);
            Console.WriteLine(value);
            Console.Read();

        }
        public static int lookupFunction(string input)
        {
            if (input == "A5")
            {
                return 10;
            }
            else
            {
                return 2;
            }
        }





    }
}
