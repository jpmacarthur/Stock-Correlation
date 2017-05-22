using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            sqlRetrieve get = new sqlRetrieve();
            Correlation calc = new Correlation();
            Dictionary<string, List<double>> test = new Dictionary<string, List<double>>();
            List<double> l1 = get.retrievePrice("PIH");
            List<double> l2 = get.retrievePrice("AAPL");
            double cor = calc.ComputeCoeff(l1.ToArray(), l2.ToArray());
            Console.WriteLine(cor);
            
        }
    }
}
