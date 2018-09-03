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
            string test = "(15+1) * (4/2) + A15";
            Evaluator.Lookup evaluate = lookupFunction;
            int value = Evaluator.Evaluate(test, evaluate);
            Console.WriteLine(value);
            Console.Read();

        }
        public static int lookupFunction(string input)
        {
            if (input == "A5")
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }





    }
}
