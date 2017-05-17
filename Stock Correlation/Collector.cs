using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Stock_Correlation
{
    public class Collector
    {
        public Dictionary<string, string> main()
        {
            downloadJson dl = new downloadJson();
            CSV_Parser par = new CSV_Parser();
            parseJson jsonP = new parseJson();
            List<string> jsonDL = dl.download(par.stockParse());

            Dictionary<string, string> paired = new Dictionary<string, string>();



            paired = jsonP.parse(jsonDL);
            return paired;

        }
    }
}
        
