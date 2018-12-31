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

    }
}
