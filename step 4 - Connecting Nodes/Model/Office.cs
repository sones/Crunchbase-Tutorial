using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Office
    {
        public String description { get; set; }
        public String address1 { get; set; }
        public String address2 { get; set; }
        public String zip_code { get; set; }
        public String city { get; set; }
        public String state_code { get; set; }
        public String country_code { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
