using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace FormulaEvaluator
{
    public class Evaluator
    {
        public delegate int Lookup(String s);
        public static int Evaluate(String input, Lookup Evaluator)
        {
            input.Trim();
            string[] substrings = Regex.Split(input, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            PrintString(substrings);
            Stack<string> operatorStack = new Stack<string>();
            Stack<int> valueStack = new Stack<int>();
            foreach (String s in substrings)
            {
                string token = s.Trim();
                switch (token)
                {
                    case "(":
                        Console.WriteLine("Left Parenthesis");
                        operatorStack.Push("(");
                        break;
                    case ")":
                        Console.WriteLine("Right Parenthesis");
                        // if top of stack is + or - 
                        if (IsPlusOrMinus(operatorStack))
                        {
                            // computing plus or minus operation
                            int computedValue = PerformPlusMinusComputation(operatorStack, valueStack);

                            //push computedValue back onto stack
                            valueStack.Push(computedValue);
                        }
                        //popping next operator off the stack, which should be a ')'
                        operatorStack.Pop();
                        //if the top of stack is / or *
                        if (IsMultiplyOrDivide(operatorStack))
                        {
                            //doing a / or * divide computation, but using two values from stack, instead of using value passed in. Setting isTokenInt to false. 
                            int computedValue = PerformMultiplyDivideComputation(operatorStack, valueStack, 0, false);
                            valueStack.Push(computedValue);
                        }
                        break;
                    case "/":
                        Console.WriteLine("Division Symbol");
                        //push '/' onto the operator stack
                        operatorStack.Push("/");
                        break;
                    case "*":
                        Console.WriteLine("Multiplication Symbol");
                        // push '*' onto the operator stack
                        operatorStack.Push("*");
                        break;
                    case "+":
                        Console.WriteLine("Plus Symbol");
                        // if '+' or '-' is at top of stack 
                        if (IsPlusOrMinus(operatorStack))
                        {
                            //computing the operation
                            int computedValue = PerformPlusMinusComputation(operatorStack, valueStack);
                            //pushing computed value onto value stack
                            valueStack.Push(computedValue);
                        }
                        //push + to operator stack
                        operatorStack.Push("+");
                        break;
                    case "-":
                        Console.WriteLine("Minus symbol");
                        // if + or - is at top of stack 
                        if (IsPlusOrMinus(operatorStack))
                        {
                            //computing the operation
                            int computedValue = PerformPlusMinusComputation(operatorStack, valueStack);
                            //push computed value onto value stack
                            valueStack.Push(computedValue);
                        }
                        //push - to operator stack
                        operatorStack.Push("-");
                        break;
                    case "":
                        Console.WriteLine("White Space");
                        break;
                    default:
                        //checking for integer token. 
                        //if IsValidTokenInt returns 0, that means that it is not a valid integer expression. 
                        int intToken = IsValidTokenInt(token);

                        if (intToken != 0)
                        {
                            //handling the token by passing in the correct value, 'intToken'
                            HandleIntOrVariable(operatorStack, valueStack, intToken);
                            Console.WriteLine("Integer");
                            break;
                        }
                        if (IsValidTokenVariable(token))
                        {
                            Console.WriteLine("Correct Variable", token);
                            int value = Evaluator(token);
                            //Handling the token by passing in the correct value from the evaluator delegate function, 'value'.
                            HandleIntOrVariable(operatorStack, valueStack, value);
                        }
                        break;
                }
            } //at the end of the input string
            //if there are no more operators
            if (operatorStack.Count == 0)
                //return the value (which should be the only one left) of the value stack. 
                return valueStack.Pop();
            else
                //else perform one last computation 
                return PerformPlusMinusComputation(operatorStack, valueStack);
        }
        /// <summary>
        /// Printing the substrings of the inputted strings for testing purposes. 
        /// </summary>
        /// <param name="substrings"></param>
        public static void PrintString(string[] substrings)
        {
            foreach (String s in substrings)
            {
                string trimmed = s.Trim();
                if (trimmed == " ")
                {
                    Console.WriteLine("/");
                }
                else
                {
                    Console.WriteLine(trimmed);
                }

            }
            //Console.Read();
        }
        public static bool IsPlusOrMinus(Stack<string> operandStack)
        {
            return (operandStack.IsOnTop("+") || operandStack.IsOnTop("-"));
        }
        public static bool IsMultiplyOrDivide(Stack<string> operandStack)
        {
            return (operandStack.IsOnTop("*") || operandStack.IsOnTop("/"));
        }
        public static int ComputeValue(int value1, int value2, string op)
        {
            switch (op)
            {
                case "+":
                    return value1 + value2;
                case "-":
                    return value1 - value2;
                case "/":
                    return value1 / value2;
                case "*":
                    return value1 * value2;
                default:
                    return 0;
            }
        }
        public static int PerformPlusMinusComputation(Stack<string> operatorStack, Stack<int> valueStack)
        {
            // pop the top two values from the value stack
            int firstValue = valueStack.Pop();
            int secondValue = valueStack.Pop();

            //pop the operator from the operator stack
            string op = operatorStack.Pop();

            //compute value
            int computedValue = ComputeValue(secondValue, firstValue, op);

            return computedValue;
        }
        public static int PerformMultiplyDivideComputation(Stack<string> operatorStack, Stack<int> valueStack, int value, bool isTokenInt)
        {
            // if the token is an integer, perform the multiply/divide computation by taking in a variable, and popping from stack
            if (isTokenInt)
            {
                int stackValue = valueStack.Pop();
                string op = operatorStack.Pop();
                int computedValue = ComputeValue(stackValue, value, op);
                return computedValue;
            }
            // if the token is not an integer, and a ')', perform the multiply/divide computation by popping the two values from the valuestack, and performing computation
            else
            {
                int firstValue = valueStack.Pop();
                int secondValue = valueStack.Pop();
                string op = operatorStack.Pop();
                int computedValue = ComputeValue(secondValue, firstValue, op);
                return computedValue;
            }

        }
        public static void HandleIntOrVariable(Stack<string> operatorStack, Stack<int> valueStack, int value)
        {
            if (IsMultiplyOrDivide(operatorStack))
            {
                int computedValue = PerformMultiplyDivideComputation(operatorStack, valueStack, value, true);
                valueStack.Push(computedValue);
            }
            else
                valueStack.Push(value);
        }
        public static int IsValidTokenInt(string token)
        {
            int value = 0;
            bool isInt = int.TryParse(token, out value);
            if (isInt && value < 0)
                throw new ArgumentException("Integer token is negative");
            return value;

        }
        public static bool IsValidTokenVariable(string token)
        {
            if (token.Length >= 2)
            {
                if (!Char.IsLetter(token[0]))
                {
                    throw new ArgumentException("The first character in the token variable is not a letter.");
                }
                else if (!Char.IsDigit(token[token.Length - 1]))
                {
                    throw new ArgumentException("The last character in the token variable is not an integer");
                }
                bool atNumbers = false;
                char previous = 'a';
                foreach (char c in token)
                {
                    if (!Char.IsLetter(c) && !Char.IsDigit(c))
                    {
                        throw new ArgumentException("Token variable contains character that is not a letter or a number");
                    }
                    else if (Char.IsLetter(c) && atNumbers && Char.IsDigit(previous))
                    {
                        throw new ArgumentException("Token variable does not end with a sequence of numbers");
                    }
                    else if (Char.IsDigit(c))
                    {
                        if (!atNumbers)
                        {
                            atNumbers = true;
                            previous = c;
                        }
                        else
                            previous = c;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Token either needs to be one of 4 operands, a valid integer, or a valid variable.");
            }
            return true;
        }
    }
    static class Extensions
    {
        public static bool IsOnTop<t>(this Stack<t> stack, t op)
        {
            if (stack.Count == 0)
                return false;
            return stack.Peek().Equals(op);
        }
    }

}