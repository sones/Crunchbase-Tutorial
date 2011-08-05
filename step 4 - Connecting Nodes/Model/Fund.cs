using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Fund
    {
        public String name { get; set; }
        public int? funded_year { get; set; }
        public int? funded_month { get; set; }
        public int? funded_day { get; set; }
        public decimal? raised_amount { get; set; }
        public String raised_currency_code { get; set; }
        public String source_url { get; set; }
        public String source_description { get; set; }

        public DateTime? Funded
        {
            get { return DateTimeHelper.getIntegersAsDateTime(funded_year, funded_month, funded_day); }
        }
    }
}
