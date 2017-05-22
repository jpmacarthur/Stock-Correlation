using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using System.Text.RegularExpressions;
using System.IO;

namespace Stock_Correlation
{
    public class CSV_Parser
    {
        public List<string> stockParse()
        {
            string nasdaq = "http://www.nasdaq.com/screening/companies-by-industry.aspx?exchange=NASDAQ&render=download";
            downloadCSV dl = new downloadCSV();
            string csvFile = dl.GetCSV(nasdaq);
            var result = Regex.Split(csvFile, "\r\n|\r\n");

            StreamWriter file = new StreamWriter(@"c:\users\pat\desktop\stockList.txt");
            List<string> res2 = new List<string>();
            List<string> finalResult = new List<string>();
            StringBuilder test = new StringBuilder();
            string temp;
            var engine = new FileHelperEngine<Orders>();
            foreach (string line in result)
            {
                temp = line.TrimEnd(',');
                test.Append(temp + Environment.NewLine);
            }
            var records = engine.ReadString(test.ToString());
            foreach (var record in records)
            {
                file.WriteLine(record.Symbol);
                finalResult.Add(record.Symbol);
            }
            return finalResult;
        }
    }
}
