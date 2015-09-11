using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{   /// <summary>
    /// Class that contains a method to evaluates arithmetic expressions written using standard infix notation.
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Method to look up a provided variable.
        /// </summary>
        /// <param name="v">Variable to be looked up.</param>
        /// <returns>An integer value that corresponds to the variable.</returns>
        public delegate int Lookup(string v);
        /// <summary>
        /// Method to check what operator is on top of the stack.
        /// </summary>
        /// <param name="stack">Stack I am looking at.</param>
        /// <param name="op1">First operator I am looking for.</param>
        /// <param name="op2">Second operator I am looking for.</param>
        /// <returns>True if does match. False if doesn't.</returns>
        public static bool hasOnTop(this Stack<string> stack, string op1, string op2)
        {
            if (stack.Count > 0)
                return ((stack.Peek().Equals(op1)) || (stack.Peek().Equals(op2)));
            else
                return false;
        }
        /// <summary>
        /// Method to parse double to integer.
        /// </summary>
        /// <param name="v">Value to be parsed.</param>
        /// <returns>Integer parsed value.</returns>
        public static int parseToInt(double v)
        {
            return Convert.ToInt32(v);
        }
        /// <summary>
        /// Method to parse string to double.
        /// </summary>
        /// <param name="v">Object from the stack.</param>
        /// <returns>Associated double value.</returns>
        public static double parseToDouble(string v)
        {
            double value;
            double.TryParse(v, out value);
            return value;
        }
        /// <summary>
        /// Print the expression on the screen.
        /// </summary>
        /// <param name="s">Array of substrings.</param>
        public static void print(string[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Console.Write(s[i]);
            }
        }
        /// <summary>
        /// Method that evaluates arithmetic expressions written using standard infix notation.
        /// </summary>
        /// <param name="exp">Expression to be evaluated.</param>
        /// <param name="var">A function used to lookup variable values.</param>
        /// <returns>Expression result.</returns>
        public static int Evaluate(string exp, Lookup var)
        {
            Stack<double> Value = new Stack<double>();
            Stack<string> Operator = new Stack<string>();

            exp = Regex.Replace(exp, " ", string.Empty);
            string[] token = Regex.Split(exp,"(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            print(token);

            for (int i = 0; i < token.Length; i++)
            {
                if (token[i] == string.Empty) continue;
                switch (token[i])
                {
                    case "*":
                    case "/":

                        Operator.Push(token[i]);
                        break;

                    case "+":
                    case "-":

                        if (hasOnTop(Operator, "+", "-"))
                        {
                            double valueA;
                            double valueB;

                            try
                            {
                                valueB = Value.Pop();
                                valueA = Value.Pop();
                            }
                            catch
                            {
                                throw new ArgumentException("Stack has fewer than two values!");
                            }

                            if (Operator.Pop().Equals("+"))
                                Value.Push(valueA + valueB);
                            else
                                Value.Push(valueA - valueB);
                            Operator.Push(token[i]);
                        }
                        else
                        {
                            Operator.Push(token[i]);
                        }
                        break;

                    case "(":

                        Operator.Push(token[i]);
                        break;

                    case ")":

                        if (hasOnTop(Operator, "+", "-"))
                        {
                            double valueA;
                            double valueB;

                            try
                            {
                                valueB = Value.Pop();
                                valueA = Value.Pop();
                            }                                
                            catch
                            {
                                throw new ArgumentException("Stack has fewer than two values!");
                            }

                            if (Operator.Pop().Equals("+"))
                                Value.Push(valueA + valueB);
                            else
                                Value.Push(valueA - valueB);

                        }
                        try
                        {
                            Operator.Pop();
                        }
                        catch
                        {
                            throw new ArgumentException("Initial parentesis not found!");
                        }
                        if (hasOnTop(Operator, "*", "/"))
                        {
                            double valueA;
                            double valueB;

                            try
                            {
                                valueB = Value.Pop();
                                valueA = Value.Pop();
                            }
                            catch
                            {
                                throw new ArgumentException("Stack has fewer than two values!");
                            }

                            if (Operator.Pop().Equals("*"))
                                Value.Push(valueA * valueB);
                            else
                                if (valueB != 0)
                                    Value.Push(valueA / valueB);
                                else
                                    throw new ArgumentException("Invalid divisor!");
                        }
                        break;

                    default:

                        if (Regex.IsMatch(token[i], "^[0-9]*$")) // if token is an integer
                            if (hasOnTop(Operator, "*", "/"))
                                if (Operator.Pop().Equals("*"))
                                {
                                    double valueA = Value.Pop();
                                    double valueB = parseToDouble(token[i]);
                                    Value.Push(valueA * valueB);
                                }
                                else
                                {
                                    double valueA;
                                    double valueB = parseToDouble(token[i]);

                                    try
                                    {
                                        valueA = Value.Pop();
                                    }
                                    catch
                                    {
                                        throw new ArgumentException("No value on the stack!");
                                    }

                                    if (valueB != 0)
                                        Value.Push(valueA / valueB);
                                    else
                                        throw new ArgumentException("Invalid divisor!");
                                }
                            else
                            {
                                Value.Push(parseToDouble(token[i]));
                            }
                        else
                            if (Regex.IsMatch(token[i], "^[a-zA-Z]*[0-9]+$")) // if token is a variable
                                if (hasOnTop(Operator, "*", "/"))
                                    if (Operator.Pop().Equals("*"))
                                    {
                                        double valueA = Value.Pop();
                                        Value.Push(valueA * var(token[i]));
                                    }
                                    else
                                    {
                                        double valueA;
                                        try
                                        {
                                            valueA = Value.Pop();
                                        }
                                        catch
                                        {
                                            throw new ArgumentException("No value on the stack!");
                                    }
                                    if (var(token[i]) != 0)
                                        Value.Push(valueA / var(token[i]));
                                    else
                                        throw new ArgumentException("Invalid divisor!");
                                }
                                else
                                    Value.Push(var(token[i]));
                                   
                        break;
                }
            }
            
            if (Operator.Count == 0)
            {
                if (!(Value.Count == 1))
                    throw new ArgumentException("There is not exactly one value on the value stack!");

                return parseToInt(Value.Pop());
            }
            else
            {
                if (!(Operator.Count == 1))
                    throw new ArgumentException("There is not exactly one operator on the operator stack!");

                if (!(Value.Count == 2))
                    throw new ArgumentException("There is not exactly two values on the value stack!");

                double valueB = Value.Pop();
                double valueA = Value.Pop();

                if (Operator.Pop().Equals("+"))
                    Value.Push(valueA + valueB);
                else
                    Value.Push(valueA - valueB);
            }

            return parseToInt(Value.Pop());
        }
    }
}
