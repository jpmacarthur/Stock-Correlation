using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Stock_Correlation
{
    public class downloadJson
    {
        public List<string> download(List<string> csv)
        {
            string sampleUrl = "http://query.yahooapis.com/v1/public/yql?q=select%20Ask,%20Symbol%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(";
            string urlEnd = "&format=json&env=store://datatables.org/alltableswithkeys";
            StringBuilder finalUrl = new StringBuilder(sampleUrl);
            List<string> jsonList = new List<string>();

            foreach (string single in csv)
            {
                if ((finalUrl.Length + (single.Length + 6) + urlEnd.Length) < 2000)
                {
                    finalUrl.Append("\"" + single + "\",%20");
                }
                else
                {
                    finalUrl.Length = finalUrl.Length - 4;
                    finalUrl.Append(")" + urlEnd);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(finalUrl.ToString());
                    req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    string json = sr.ReadToEnd();
                    sr.Close();
                    jsonList.Add(json);
                    finalUrl = new StringBuilder(sampleUrl);
                    }
                    
                }

            return jsonList;
            }
        public string download(string csv)
        {
            string sampleUrl = "http://query.yahooapis.com/v1/public/yql?q=select%20Ask,%20Symbol%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(";
            string urlEnd = "&format=json&env=store://datatables.org/alltableswithkeys";
            StringBuilder finalUrl = new StringBuilder(sampleUrl);
                    finalUrl.Append("\"" + csv + "\"");
                    finalUrl.Append(")" + urlEnd);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(finalUrl.ToString());
                    req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    string dlFile = sr.ReadToEnd();
                    sr.Close();
                

            

            return dlFile;
        }

    }
    }

