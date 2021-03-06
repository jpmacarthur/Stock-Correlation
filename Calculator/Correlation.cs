﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    public class Correlation
    {
        public double calcCorr(Dictionary<string, List<double>> stock)
        {

            double value = 0;
            foreach(KeyValuePair<string, List<double>> lis in stock)
            {
                
                foreach(double val in lis.Value)
                {
                    value += val;
                }
                
            }



            return value;
        }
        public double ComputeCoeff(List<double> values1, List<double> values2)
        {
            while (values1.Count > values2.Count) { values1.RemoveAt(0); }
            while (values2.Count > values2.Count) { values2.RemoveAt(0); }

            var avg1 = values1.Average();
            var avg2 = values2.Average();

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => Math.Pow((x - avg1), 2.0));
            var sumSqr2 = values2.Sum(y => Math.Pow((y - avg2), 2.0));

            var result = sum1 / Math.Sqrt(sumSqr1 * sumSqr2);

            return result;
        }
    }

}
