using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class IPO
    {
        public decimal? valuation_amount { get; set; }
        public String valuation_currency_code { get; set; }
        public int? pub_year { get; set; }
        public int? pub_month { get; set; }
        public int? pub_day { get; set; }
        public String stock_symbol { get; set; }

        public DateTime? PublishedAt
        {
            get { return DateTimeHelper.getIntegersAsDateTime(pub_year, pub_month, pub_day); }
        }
    }
}
