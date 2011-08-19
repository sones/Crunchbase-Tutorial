using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Degree
    {
        public String degree_type { get; set; }
        public String subject { get; set; }
        public String institution { get; set; }
        public int? graduated_year { get; set; }
        public int? graduated_month { get; set; }
        public int? graduated_day { get; set; }

        public DateTime? GraduatedAt
        {
            get { return DateTimeHelper.getIntegersAsDateTime(graduated_year, graduated_month, graduated_day); }
        }

    }
}
