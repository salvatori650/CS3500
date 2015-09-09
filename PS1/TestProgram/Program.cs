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
        static void Main(string[] args)
        {
            Console.Write("\nResult: " + Evaluator.Evaluate("5 * 4 /(1   +7) -   2", null));
            string t = Console.ReadLine();
        }
    }
}
