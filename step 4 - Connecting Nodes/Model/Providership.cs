using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Providership
    {
        public String title { get; set; }
        public bool? is_past { get; set; }
        public Permalink provider { get; set; }
    }
}
