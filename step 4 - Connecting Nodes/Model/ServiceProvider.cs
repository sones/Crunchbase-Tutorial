using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class ServiceProvider
    {
        public String name { get; set; }
        public String permalink { get; set; }
        public String crunchbase_url { get; set; }
        public String homepage_url { get; set; }
        public String phone_number { get; set; }
        public String email_address { get; set; }
        public String tag_list { get; set; }
        public String alias_list { get; set; }
        public String created_at { get; set; }
        public String updated_at { get; set; }
        public String overview { get; set; }
        public Image image { get; set; }
        public List<Office> offices { get; set; }
        public List<Link> external_links { get; set; }

        public DateTime? CreatedAt
        {
            get { return created_at.AsDateTime(); }
        }

        public DateTime? UpdatedAt
        {
            get { return updated_at.AsDateTime(); }
        }


    }
}
