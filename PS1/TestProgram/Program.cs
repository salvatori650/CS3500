using System;
using System.Collections.Generic;
using FormulaEvaluator;

namespace TestProgram
{
    class Program
    {
        public static class Tester
        {
            private static int Lookup(string var)
            {
                var values = new Dictionary<string, int>
                {
                    {"d3", 10}, {"e5" , 5 }
                };
                if (!values.ContainsKey(var))
                    throw new ArgumentException("Variable has no value!");
                return values[var];
            }
            static void Main(string[] args)
            {
                Console.Write(" = " + Evaluator.Evaluate("(d3-e5)*10/(5+5)", Lookup));
                Console.Read();
            }
        }
    }
}
