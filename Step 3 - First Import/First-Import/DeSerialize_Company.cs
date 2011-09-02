using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace First_Import
{
    public class Competitor
    {
        public String name;
        public String permalink;
    }

    public class CompetitorList
    {
        public List<Competitor> competitor;
    }

    public class Company
    {
        public String name;
        public String alias_list;
        public String blog_feed_url;
        public String blog_url; 
        public String category_code; 
        public String created_at; 
        public String crunchbase_url; 
        public String deadpooled_year;
        public String deadpooled_month;
        public String deadpooled_day;
        public String description; 
        public String email_address; 
        public String founded_year;
        public String founded_month;
        public String founded_day;
        public String homepage_url; 
        public String number_of_employees; 
        public String overview; 
        public String permalink; 
        public String phone_number; 
        public String tag_list; 
        public String twitter_username;
        public String updated_at;
        public List<Dictionary<string,Competitor>> Competitions;
    }

    public static class DeSerializeCompany
    {
        //this is how we call out to crunchbase to get their full list of companies
        public static Company DeSerialize(String Filename)
        {
            
			String jsonStream = File.ReadAllText(Filename);
            jsonStream = new CorrectOutput().output(jsonStream);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            Company jsondeserialized = ser.Deserialize<Company>(jsonStream);
            return jsondeserialized;
        }

    }
}
