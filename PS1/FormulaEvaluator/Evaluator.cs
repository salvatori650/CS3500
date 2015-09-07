using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public delegate int Lookup(String v); 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns></returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {   
            System.Collections.Stack Values = new System.Collections.Stack();
            System.Collections.Stack Operators = new System.Collections.Stack();
            int first = 0;

            for (int i=0; i < exp.Length; i++)
            {
                switch (exp[i])
                {
                    case '*':
                    case '/':
                    case '+':
                    case '-':
                    case '(':
                    case ')':
                        Operators.Push(exp[i]);
                        first = i == 0 ? 1 : 0;
                        break;
                    default:
                        Values.Push(int.Parse(exp[i]));
                        first = i == 0 ? 0 : 1;
                        break;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public static int Operation(int o)
            {
                while ((Values.Count > 0) || (Operators.Count > 0))
                {
                    if (first == 0)
                        Values.Pop(); Operators.Pop();
                    if (first == 1)
                        Values.Pop(); Operators.Pop();
                }
                return 0;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="exp"></param>
            /// <returns></returns>
            public static String RemoveWhitespaces(String exp)
            {
                return exp.Replace(" ", string.Empty);
            }
        }
    }
}
