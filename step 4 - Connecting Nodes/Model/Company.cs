using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crunchbase.Model
{
    public class Company
    {
        public String name { get; set; }
        public String permalink { get; set; }
        public String crunchbase_url { get; set; }
        public String homepage_url { get; set; }
        public String blog_url { get; set; }
        public String blog_feed_url { get; set; }
        public String twitter_username { get; set; }
        public String category_code { get; set; }
        public int? number_of_employees { get; set; }
        public int? founded_year { get; set; }
        public int? founded_month { get; set; }
        public int? founded_day { get; set; }
        public int? deadpooled_year { get; set; }
        public int? deadpooled_month { get; set; }
        public int? deadpooled_day { get; set; }
        public String deadpooled_url { get; set; }
        public String tag_list { get; set; }
        public String alias_list { get; set; }
        public String email_address { get; set; }
        public String phone_number { get; set; }
        public String description { get; set; }
        public String created_at { get; set; }
        public String updated_at { get; set; }
        public String overview { get; set; }
        public Image image { get; set; }
        public List<Permalink> products { get; set; }
        public List<Relationship> relationships { get; set; }
        public List<Dictionary<string, Permalink>> competitions { get; set; }
        public List<FundingRound> funding_rounds { get; set; }
        public List<Providership> providerships { get; set; }
        public List<Acquisition> acquisitions { get; set; }
        public List<Office> offices { get; set; }
        public List<Milestone> milestones { get; set; }
        public IPO ipo { get; set; }
        public List<Video> video_embeds { get; set; }
        public List<Image> screenshots { get; set; }
        public List<Link> external_links { get; set; }

        public DateTime? CreatedAt
        {
            get { return created_at.AsDateTime(); }
        }

        public DateTime? UpdatedAt
        {
            get { return updated_at.AsDateTime(); }
        }

        public DateTime? FoundedAt
        {
            get { return DateTimeHelper.getIntegersAsDateTime(founded_year, founded_month, founded_day); }
        }

        public DateTime? DeadPooledAt
        {
            get { return DateTimeHelper.getIntegersAsDateTime(deadpooled_year, deadpooled_month, deadpooled_day); }
        }

    }
}
