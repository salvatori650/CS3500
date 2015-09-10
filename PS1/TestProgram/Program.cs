using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Console.Write("\n\nResult: " + Evaluator.Evaluate("(d3/e5)", Lookup));
                Console.Read();
            }
        }
    }
}
