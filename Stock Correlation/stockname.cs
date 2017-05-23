using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock_Correlation
{
    public interface IStockViewModel
    {
        IEnumerable<stockname>Stocks { get; }
        stockname SelectedStock { get; set; }
    }
    public class stockname
    {
        public string symbol { get; set; }
        public string name { get; set;}
        public stockname(string sym, string nam)
        {
            symbol = sym;
            name = nam;
        }
    }
}
