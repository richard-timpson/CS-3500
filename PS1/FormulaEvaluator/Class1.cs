using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

/// <summary>
/// Used to evaluate formulas. 
/// </summary>
namespace FormulaEvaluator
{
    /// <summary>
    /// Class for evaluting infix expressions
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// The delegate declarator for a lookup function that will 
        /// 'lookup' the value of a variable used in the expression. 
        /// </summary>
        /// <param name="s">The name of the variable</param>
        /// <returns>The value of the variable</returns>
        public delegate int Lookup(String s);
        /// <summary>
        /// The main evaluate function for the infix expression
        /// It breaks the expression into substrings and uses
        /// a switch case statement to handle the different cases
        /// for the different tokens. It will use the 
        /// Lookup evaluator to perform the correct calculations on variables.
        /// Also checks for valid tokens in the expressions and throws exceptions
        /// for ones that aren't.
        /// </summary>
        /// <param name="input">The expression to evaluate</param>
        /// <param name="Evaluator">The Lookup delegate for variable tokens</param>
        /// <returns>The value of the infix expression</returns>
        public static int Evaluate(String input, Lookup Evaluator)
        {
            input.Trim();
            string[] substrings = Regex.Split(input, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            //PrintString(substrings);
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
                        if (operatorStack.Count == 0 || operatorStack.Peek() != "(")
                        {
                            throw new ArgumentException("Parenthesis are not set up correctly");
                        }
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
                        int intToken;
                        bool isValidInt = TryTokenInt(token, out intToken);

                        if (isValidInt)
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
            { 
                //return the value (which should be the only one left) of the value stack. 
                if (valueStack.Count == 0)
                {
                    throw new ArgumentException("Not a correct amount of operands with operators");
                }
                return valueStack.Pop();
            }
            else
            {
                if (operatorStack.Count != 1)
                {
                    throw new ArgumentException("Isn't one operator on the value stack at the end of the algorithm");
                }
                else if (valueStack.Count !=2 )
                {
                    throw new ArgumentException("Aren't 2 operators in the value stack at the end of the algorithm");
                }
                //else perform one last computation 
                return PerformPlusMinusComputation(operatorStack, valueStack);
            }
        }
        /// <summary>
        /// Printing the substrings of the inputted strings for testing purposes. 
        /// </summary>
        /// <param name="substrings">The array of strings to be printed</param>
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
        }
        /// <summary>
        /// Checks the top of the stack, using the IsOnTop stack extension, to see if the variable is either a '+' or '-'
        /// </summary>
        /// <param name="operandStack">The operator stack to check</param>
        /// <returns>True or False, based on the IsOnTop extension check for both operators</returns>
        public static bool IsPlusOrMinus(Stack<string> operandStack)
        {
            return (operandStack.IsOnTop("+") || operandStack.IsOnTop("-"));
        }
        /// <summary>
        /// Checks the top of the stack, using the IsOnTop stack extension, to see if the variable is either a '*' or '/'. 
        /// Similar to IsMultiplyOrDivide
        /// </summary>
        /// <param name="operandStack">The operator stack to check</param>
        /// <returns>True or False, based on the IsOnTop extension check for both operators</returns>
        public static bool IsMultiplyOrDivide(Stack<string> operandStack)
        {
            return (operandStack.IsOnTop("*") || operandStack.IsOnTop("/"));
        }
        /// <summary>
        /// Makes simple computations based on the values passed in and the operator to use. 
        /// Similar to IsPlusOrMinus
        /// </summary>
        /// <param name="value1">One of the values for computation</param>
        /// <param name="value2">The other value for computation</param>
        /// <param name="op">The operation to perform</param>
        /// <returns>The value of the computation after it is finished. If an incorrect operator was passed in, 0 will be returned</returns>
        public static int ComputeValue(int value1, int value2, string op)
        {
            switch (op)
            {
                case "+":
                    return value1 + value2;
                case "-":
                    return value1 - value2;
                case "/":
                    if (value2 == 0) 
                    {
                        throw new ArgumentException("Division by 0");
                    }
                    return value1 / value2;
                case "*":
                    return value1 * value2;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Uses the Compute Value function to perform a plus or minus computation. 
        /// It does so by popping two values from the valueStack given,
        /// and using the operation from the operator stack. 
        /// Similar to PerformMultiplyDivideComputation
        /// </summary>
        /// <param name="operatorStack">The operator </param>
        /// <param name="valueStack">The value stack</param>
        /// <returns>The computed value</returns>
        public static int PerformPlusMinusComputation(Stack<string> operatorStack, Stack<int> valueStack)
        {
            if (valueStack.Count < 2)
            {
                throw new ArgumentException("Trying to add or minus without enough values");
            }
            // pop the top two values from the value stack
            int firstValue = valueStack.Pop();
            int secondValue = valueStack.Pop();

            //pop the operator from the operator stack
            string op = operatorStack.Pop();

            //compute value
            int computedValue = ComputeValue(secondValue, firstValue, op);

            return computedValue;
        }
        /// <summary>
        /// Uses the Compute Value function to perform a plus or minus computation. 
        /// It does so by popping two values from the valueStack given,
        /// or by popping only one value from the valueStack, and using a value passed in.
        /// Similar to PerformMultiplyDivideComputation
        /// </summary>
        /// <param name="operatorStack">Operator Stack</param>
        /// <param name="valueStack">Value Stack</param>
        /// <param name="value">Value to use for computation, only if you don't want to use two variables from value stack</param>
        /// <param name="isTokenInt">If True, it will assume the value passed will be used for computation. If false, it will calculate with two values from value stack</param>
        /// <returns></returns>
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
        /// <summary>
        /// Performs the same computation for both integers and variables, when given the value. 
        /// </summary>
        /// <param name="operatorStack">Operator Stack</param>
        /// <param name="valueStack">value Stack</param>
        /// <param name="value">The value used for the computation. should come from integer token or variable token</param>
        public static void HandleIntOrVariable(Stack<string> operatorStack, Stack<int> valueStack, int value)
        {
            if (IsMultiplyOrDivide(operatorStack))
            {
                if (valueStack.Count == 0)
                {
                    throw new ArgumentException("Value stack is empty, trying to perform integer operation");
                }
                int computedValue = PerformMultiplyDivideComputation(operatorStack, valueStack, value, true);
                valueStack.Push(computedValue);
            }
            else
                valueStack.Push(value);
        }
        /// <summary>
        /// Checks if a token is a valid integer token. 
        /// Will throw argument exception if it isn't. 
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>0 if not valid, or the value of the integer if it is valid</returns>
        public static bool TryTokenInt(string token, out int intToken)
        {

            bool isInt = int.TryParse(token, out intToken);
            if (isInt && intToken < 0)
                throw new ArgumentException("Integer token is negative");
            return isInt;

        }
        /// <summary>
        /// Checks if a token is a valid variable token. 
        /// Will throw ArgumentExceptions if it isn't
        /// </summary>
        /// <param name="token"The token to check></param>
        /// <returns>True if valid token. </returns>
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
    /// <summary>
    /// Providing one extension for the Stack class. 
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Extension for the stack class to check if a certain value is at the top of the stack. 
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="stack">The stack to check</param>
        /// <param name="op">The value to check</param>
        /// <returns>True or false, depending on the value of the stack peek and the value passed in. </returns>
        public static bool IsOnTop<t>(this Stack<t> stack, t op)
        {
            if (stack.Count == 0)
                return false;
            return stack.Peek().Equals(op);
        }
    }

}