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

            eq = "2 + 2";
            double answer = Parser.SplitAndCalc(eq, ref index);
            Console.WriteLine($"equation {eq} = {answer}");

            index = 0;
            eq = "2^(2 + 3) + 2 * 2";
            answer = Parser.SplitAndCalc(eq, ref index);
            Console.WriteLine($"equation {eq} = {answer}");


        }
    }
}
