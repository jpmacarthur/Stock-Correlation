using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace Stock_Correlation
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    class delim
    {
        public int Symbol;
        public string name;
        public int LastSale;
        public double MarketCap;




    }
}
