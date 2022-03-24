using StringParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringParser
{
    public class FuncAprox
    {
        private Parser parser = new Parser();
        private int index = 0;
        public double PointFrom { get; private set; }
        public double PointTo { get; private set; }
        public double Step { get; private set; }

        public List<double> Points { get; set; } = new List<double>();


        public FuncAprox(double from = 0, double to = 1, double step = 0.1)
        {
            PointFrom = from;
            PointTo = to;
            Step = step;
        }

        public void CalcFunction(string data)
        {
            Points.Clear();
            for (double i = PointFrom; i < PointTo; i += Step)
            {
                double temp = Parser.SplitAndCalc(data, ref index, i);
                Points.Add(temp);
                index = 0;
            }
        }
    }
}
