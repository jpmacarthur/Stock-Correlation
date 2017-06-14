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
            List<double> l1 = get.retrievePrice("PIH", "127.0.0.1", "StockInfo", "theblob", "pmacarthur");
            List<double> l2 = get.retrievePrice("AAPL", "127.0.0.1", "StockInfo", "theblob", "pmacarthur");
            double cor = calc.ComputeCoeff(l1, l2);
            Console.WriteLine(cor);
            
        }
    }
}
