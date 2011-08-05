using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Person
    {
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String permalink { get; set; }
        public String crunchbase_url { get; set; }
        public String homepage_url { get; set; }
        public String birthplace { get; set; }
        public String twitter_username { get; set; }
        public String blog_url { get; set; }
        public String blog_feed_url { get; set; }
        public String affiliation_name { get; set; }
        public int? born_year { get; set; }
        public int? born_month { get; set; }
        public int? born_day { get; set; }
        public String tag_list { get; set; }
        public String alias_list { get; set; }
        public String created_at { get; set; }
        public String updated_at { get; set; }
        public String overview { get; set; }
        public Image image { get; set; }
        public List<Degree> degrees { get; set; }
        public List<Milestone> milestones { get; set; }
        public List<Video> video_embeds { get; set; }
        public List<Link> external_links { get; set; }
        public List<WebPresence> web_presences { get; set; }

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
