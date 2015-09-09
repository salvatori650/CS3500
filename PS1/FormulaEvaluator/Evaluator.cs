using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

// need to work on exceptions

namespace FormulaEvaluator
{   /// <summary>
    /// 
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public delegate int Lookup(string v);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool hasOnTop(Stack stack, string op)
        {
            if (stack.Count > 0)
            {
                if (stack.Peek().Equals(op))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int parseToInt(object v)
        {
            return int.Parse(v.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double parseToDouble(object v)
        {
            return double.Parse(v.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns></returns>
        public static int Evaluate(string exp, Lookup variableEvaluator)
        {
            Stack Value = new Stack();
            Stack Operator = new Stack();

            exp = Regex.Replace(exp, " ", string.Empty);
            Console.WriteLine(exp + "\n");

            string[] token = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            for (int i = 0; i <= exp.Length; i++)
            {
                switch (token[i])
                {
                    case "*":
                    case "/":
                        Console.Write(token[i]);
                        Operator.Push(token[i]);
                        break;
                    case "+":
                    case "-":
                        if ((hasOnTop(Operator, "+")) || (hasOnTop(Operator, "-")))
                        {
                            int valueA = parseToInt(Value.Pop());
                            int valueB = parseToInt(Value.Pop());
                            if (Operator.Pop().Equals("+"))
                            {
                                Console.Write(valueA + valueB);
                                Value.Push(valueA + valueB);
                            }
                            else
                            {
                                Console.Write(valueA - valueB);
                                Value.Push(valueA - valueB);
                            }
                            Operator.Push(token[i]);
                        }
                        else
                        {
                            Console.Write(token[i]);
                            Operator.Push(token[i]);
                        }
                        break;
                    case "(":
                        Console.Write(token[i]);
                        Operator.Push(token[i]);
                        break;
                    case ")":
                        if ((hasOnTop(Operator, "+")) || (hasOnTop(Operator, "-")))
                        {
                            int valueA = parseToInt(Value.Pop());
                            int valueB = parseToInt(Value.Pop());
                            {
                                if (Operator.Pop().Equals("+"))
                                {
                                    Console.Write(valueA + valueB);
                                    Value.Push(valueA + valueB);
                                }
                                else
                                {
                                    Console.Write(valueA - valueB);
                                    Value.Push(valueA - valueB);
                                }
                            }
                            Operator.Pop();
                        }
                        if ((hasOnTop(Operator, "*")) || (hasOnTop(Operator, "/")))
                        {
                            double valueA = parseToDouble(Value.Pop());
                            double valueB = parseToDouble(Value.Pop());
                            if (Operator.Pop().Equals("*"))
                            {
                                Console.Write(valueA * valueB);
                                Value.Push(valueA * valueB);
                            }
                            else
                            {
                                Console.Write(valueA / valueB);
                                Value.Push(valueA / valueB);
                            }
                        }
                        break;
                    default:
                        if (Regex.IsMatch(token[i],@"^\d$")) // if token is an integer
                        {
                            if ((hasOnTop(Operator, "*")) || (hasOnTop(Operator, "/")))
                                if (Operator.Pop().Equals("*"))
                                {
                                    int valueA = parseToInt(Value.Pop());
                                    Console.Write(parseToInt(token[i]) * valueA);
                                    Value.Push(parseToInt(token[i]) * valueA);
                                }
                                else
                                {
                                    int valueA = parseToInt(Value.Pop());
                                    Console.Write(parseToDouble(token[i]) / valueA);
                                    Value.Push(parseToDouble(token[i]) / valueA);
                                }
                            else
                            {
                                Console.Write(token[i]);
                                Value.Push(parseToInt(token[i]));
                            }
                        }
                        else
                        {
                            if (Regex.IsMatch(token[i], @"^\s$")) // if token is a variable
                                if ((hasOnTop(Operator, "*")) || (hasOnTop(Operator, "/")))
                                    if (Operator.Pop().Equals("+"))
                                        Value.Push(variableEvaluator(token[i]) * parseToInt(Value.Pop()));
                                    else
                                        Value.Push(variableEvaluator(token[i]) / parseToInt(Value.Pop()));
                                else
                                    Value.Push(token[i]);
                        }
                        break;
                }
            }

            int value;
            if (Operator.Count == 0)
            {
                value = parseToInt(Value.Pop());
                return value;
            }
            else
            {
                int valueA = parseToInt(Value.Pop());
                int valueB = parseToInt(Value.Pop());
                if (Operator.Pop().Equals("+"))
                {
                    Console.Write("\n" + valueA + valueB);
                    Value.Push(valueA + valueB);
                }
                else
                {
                    Console.Write("\n" + valueA + valueB);
                    Value.Push(valueA - valueB);
                }
            }
            value = parseToInt(Value.Pop());
            return value;
        }
    }
}
