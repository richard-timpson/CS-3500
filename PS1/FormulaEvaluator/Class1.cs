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
                string trimmed = s.Trim();
                switch (trimmed)
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
                            int computedValue = PerformMultiplyDivideComputation(operatorStack,  valueStack, 0, false);
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
                            int computedValue = PerformPlusMinusComputation(operatorStack, valueStack);
                            //push computed value onto value stack
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
                            int computedValue = PerformPlusMinusComputation(operatorStack, valueStack);
                            //push computed value onto value stack
                            valueStack.Push(computedValue);
                        }
                        //push - to operator stack
                        operatorStack.Push("-");
                        break;
                    case " ":
                        Console.WriteLine("White Space");
                        break;
                    default:

                        int value;
                        //checking for integer token. 
                        if (int.TryParse(trimmed, out value))
                        {
                            Console.WriteLine("Integer");
                            HandleIntOrVariable(operatorStack, valueStack, value);
                            break;
                        }
                        // checking for variable token
                        else if (trimmed.Length == 2 && Char.IsLetter(trimmed, 0) && Char.IsDigit(trimmed[1]))
                        {
                            Console.WriteLine("Correct Variable", trimmed);
                            value = Evaluator(trimmed);
                            HandleIntOrVariable(operatorStack, valueStack, value);
                            break;
                        }
                        //handling exceptions


                        //checking if the variable string's length is greater than 0
                        else if (trimmed.Length > 2)
                        {
                            throw new System.ArgumentException("Variable Length is longer than 2");
                        }
                        //Checking if the first character in the variable input is a letter
                        else if (trimmed.Length == 2 && Char.IsLetter(trimmed, 0) == false)
                        {
                            throw new System.ArgumentException("The first character in the variable is not a letter");
                        }
                        // Checking if the second number in the variable input is a number
                        else if (trimmed.Length == 2 && Char.IsDigit(trimmed[1]) == false)
                        {
                            throw new System.ArgumentException("The second character in the variable is not a number");
                        }
                        break;

                }
            }
            if (operatorStack.Count == 0)
                return valueStack.Pop();
            else
                return PerformPlusMinusComputation(operatorStack, valueStack);
        }

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
            int computedValue = ComputeValue(firstValue, secondValue, op);

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
                int value1 = valueStack.Pop();
                int value2 = valueStack.Pop();
                string op = operatorStack.Pop();
                int computedValue = ComputeValue(value1, value2, op);
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
    }
    static class Extensions
    {
        public static bool IsOnTop<t>(this Stack<t> stack, t  op)
        {
            if (stack.Count == 0)
                return false;
            return stack.Peek().Equals(op);
        }
    }
    
}