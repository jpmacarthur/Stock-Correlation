using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock_Correlation
{
    public class parseJson
    {
        public Dictionary<string, string> parse(List<string> json)
        {
            Dictionary<string, string> paired = new Dictionary<string, string>();
            foreach (string stock in json)
            {
                string temp;
                int start = stock.IndexOf("\"quote\":");
                temp = stock.Substring(start + 8);
                temp = temp.TrimEnd('}');
                List<jsonStock> stk = JsonConvert.DeserializeObject<List<jsonStock>>(temp);
                foreach (jsonStock a in stk)
                    paired.Add(a.Symbol, a.Ask);

            }
            return paired;
        }
        public Dictionary<string, string> parse(string json)
        {
            Dictionary<string, string> paired = new Dictionary<string, string>();

            string temp;
            int start = json.IndexOf("\"quote\":");
            temp = json.Substring(start + 8);
            temp = temp.TrimEnd('}');
            temp = temp + "}";
            jsonStock stk = JsonConvert.DeserializeObject<jsonStock>(temp);

            paired.Add(stk.Symbol, stk.Ask);

            return paired;
        }
    }
}
