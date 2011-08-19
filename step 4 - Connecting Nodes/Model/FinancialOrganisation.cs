using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class FinancialOrganization
    {
        public String name { get; set; }
        public String permalink { get; set; }
        public String crunchbase_url { get; set; }
        public String homepage_url { get; set; }
        public String blog_url { get; set; }
        public String blog_feed_url { get; set; }
        public String twitter_username { get; set; }
        public String phone_number { get; set; }
        public String description { get; set; }
        public String email_address { get; set; }
        public int? number_of_employees { get; set; }
        public int? founded_year { get; set; }
        public int? founded_month { get; set; }
        public int? founded_day { get; set; }
        public String tag_list { get; set; }
        public String alias_list { get; set; }
        public String created_at { get; set; }
        public String updated_at { get; set; }
        public String overview { get; set; }
        public Image image { get; set; }
        public List<Office> offices { get; set; }
        public List<Relationship> relationships { get; set; }
        public List<Milestone> milestones { get; set; }
        public List<Providership> providerships { get; set; }
        public List<Fund> funds { get; set; }
        public List<Video> video_embeds { get; set; }
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
