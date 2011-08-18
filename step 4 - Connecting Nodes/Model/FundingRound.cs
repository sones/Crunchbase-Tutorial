using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class FundingRound
    {
        public String round_code { get; set; }
        public String source_url { get; set; }
        public String source_description { get; set; }
        public decimal? raised_amount { get; set; }
        public String raised_currency_code { get; set; }
        public int? funded_year { get; set; }
        public int? funded_month { get; set; }
        public int? funded_day { get; set; }
        public List<Dictionary<String, Permalink>> investments{ get; set; }

        public DateTime? FundedAt
        {
            get { return DateTimeHelper.getIntegersAsDateTime(funded_year, funded_month, funded_day); }
        }
    }
}
