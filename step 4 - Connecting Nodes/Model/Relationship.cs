using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Relationship
    {
        public bool? is_past { get; set; }
        public String title { get; set; }
        public Person person { get; set; }
    }
}
