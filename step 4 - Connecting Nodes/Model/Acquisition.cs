using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Acquisition
    {
        public decimal? price_amount { get; set; }
        public String price_currency_code { get; set; }
        public String term_code { get; set; }
        public String source_url { get; set; }
        public String source_description { get; set; }
        public int? acquired_year { get; set; }
        public int? acquired_month { get; set; }
        public int? acquired_day { get; set; }
        public Permalink company { get; set; }

        public DateTime? AcquiredAt
        {
            get { return DateTimeHelper.getIntegersAsDateTime(acquired_year, acquired_month, acquired_day); }
        }
    }
}
