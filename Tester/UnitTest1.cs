using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Stock_Correlation;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft;
using Newtonsoft.Json;

namespace Tester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string sampleUrl = "http://query.yahooapis.com/v1/public/yql?q=select%20Ask,%20Symbol%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(";
            string urlEnd = "&format=json&env=store://datatables.org/alltableswithkeys";
            StringBuilder finalUrl = new StringBuilder(sampleUrl);
            

            List<string> stocks = new List<string>();
            List<string> jsonList = new List<string>();
            Dictionary<string, string> paired = new Dictionary<string, string>();
            CSV_Parser par = new CSV_Parser();
            stocks = par.getSymbols();
            foreach(string single in stocks)
            {
                if ((finalUrl.Length + (single.Length+11) + urlEnd.Length) < 2000)
                {
                    finalUrl.Append("%22" + single + "%22,%20");
                }
                else
                {
                    finalUrl.Length = finalUrl.Length - 4;
                    finalUrl.Append(")" +urlEnd);
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";
                        var json = wc.DownloadString(finalUrl.ToString());
                        jsonList.Add(json);

                    }
                    finalUrl = new StringBuilder(sampleUrl);
                }
            }
            foreach (string stock in jsonList)
            {
                string temp;
                int start = stock.IndexOf("\"quote\":");
                temp = stock.Substring(start + 8);
                temp = temp.TrimEnd('}');
                List<jsonStock> stk = JsonConvert.DeserializeObject<List<jsonStock>>(temp);
                foreach (jsonStock a in stk)
                    paired.Add(a.Symbol, a.Ask);

            }
        }
        [TestMethod]
        public void editjson()
        {
            var txt = File.ReadAllLines(@"c:\users\pat\desktop\delim.txt");
            var fin = Regex.Split(txt[0], ",");
            List<string> temp = new List<string>();
            string temp2;
            StringBuilder sb = new StringBuilder();
            int ind;
            foreach(string line in fin)
            {
                ind = line.IndexOf(':');
                temp2 = line.Substring(0, ind);
                temp2 = temp2.TrimStart('"');
                temp2 = temp2.TrimEnd('"');
                temp.Add(temp2);
            }
            File.WriteAllLines(@"c:\users\pat\desktop\delim.txt", temp);

        }
    }
}
