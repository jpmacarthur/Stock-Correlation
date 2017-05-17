using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace Stock_Correlation
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    [IgnoreEmptyLines]
    class Orders
    {
        [FieldQuoted]
        public string Symbol;
        [FieldQuoted]
        public string name;
        [FieldQuoted]
        public string LastSale;
        [FieldQuoted]
        public string MarketCap;
        [FieldQuoted]
        public string ADR_TSO;
        [FieldQuoted]
        public string IPOYear;
        [FieldQuoted]
        public string Sector;
        [FieldQuoted]
        public string Industry;
        [FieldQuoted]
        public string SumQuote;



    }
}
