using System;
using System.Collections.Generic;
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
                if ((finalUrl.Length + (single.Length + 11) + urlEnd.Length) < 2000)
                {
                    finalUrl.Append("%22" + single + "%22,%20");
                }
                else
                {
                    finalUrl.Length = finalUrl.Length - 4;
                    finalUrl.Append(")" + urlEnd);
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)");
                        var json = wc.DownloadString(finalUrl.ToString());
                        jsonList.Add(json);

                    }
                    finalUrl = new StringBuilder(sampleUrl);
                }
            }
            return jsonList;
        }
    }
}

