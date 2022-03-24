using System;
using StringParser;

namespace ParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int index = 0;
            string eq = "";
            double answer = 0;

            eq = "2 ^ (3 + 5) - 10 / 2";
            answer = Parser.SplitAndCalc(eq, ref index);
            Console.WriteLine($"equation {eq} = {answer:F3}");

            //index = 0;
            //eq = "2^(2 + 3) + 2 * 2";
            //answer = Parser.SplitAndCalc(eq, ref index);
            //Console.WriteLine($"equation {eq} = {answer:F3}");


            //Cell.AddAction('$', new MathActions(DivisionByInteger), 2);
            //index = 0;
            //eq = "5 $ 1.9";
            //answer = Parser.SplitAndCalc(eq, ref index);
            //Console.WriteLine($"equation {eq} = {answer:F3}");

            //Parser.AddFunction("cos", new MathFunctions(Cos));
            //index = 0;
            //eq = "cos(0.5 + 0.5)";
            //answer = Parser.SplitAndCalc(eq, ref index);
            //Console.WriteLine($"equation {eq} = {answer:F3}");
        }

        private static void Cos(Cell cell)
        {
            cell.Value = Math.Cos(cell.Value / 180D * Math.PI);
        }

        private static void DivisionByInteger(Cell left, Cell right)
        {
            left.Value = (int)(left.Value / right.Value);
        }
    }
}
