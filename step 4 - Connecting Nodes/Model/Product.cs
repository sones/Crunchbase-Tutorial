using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Product
    {
        public String name { get; set; }
        public String permalink { get; set; }
        public String crunchbase_url { get; set; }
        public String homepage_url { get; set; }
        public String blog_url { get; set; }
        public String blog_feed_url { get; set; }
        public String twitter_username { get; set; }
        public String stage_code { get; set; } 
        public String deadpooled_url { get; set; }
        public String invite_share_url { get; set; }
        public String tag_list { get; set; }
        public String alias_list { get; set; }
        public int? deadpooled_year { get; set; }
        public int? deadpooled_month { get; set; }
        public int? deadpooled_day { get; set; }
        public int? launched_year { get; set; }
        public int? launched_month { get; set; }
        public int? launched_day { get; set; }
        public String created_at { get; set; }
        public String updated_at { get; set; }
        public String overview { get; set; }
        public Permalink company { get; set; }
        public List<Milestone> milestones { get; set; }
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
