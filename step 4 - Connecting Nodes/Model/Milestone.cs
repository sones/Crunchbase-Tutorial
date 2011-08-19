using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Milestone
    {
        public String description { get; set; }
        public int? stoned_year { get; set; }
        public int? stoned_month { get; set; }
        public int? stoned_day { get; set; }
        public String source_url { get; set; }
        public String source_description { get; set; }

        public DateTime? StonedAt
        {
            get { return DateTimeHelper.getIntegersAsDateTime(stoned_year, stoned_month, stoned_day); }
        }
    }
}
