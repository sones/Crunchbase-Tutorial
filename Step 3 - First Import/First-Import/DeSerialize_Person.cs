using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace First_Import
{
    public class Person
    {
        public String affiliation_name;
        public String alias_list;
        public String birthplace;
        public String blog_feed_url;
        public String blog_url;
        public String born_year;
        public String born_month;
        public String born_day;
        public String created_at;
        public String crunchbase_url;
        public String first_name;
        public String homepage_url;
        public String last_name;
        public String overview;
        public String permalink;
        public String tag_list;
        public String twitter_username;
        public String updated_at; 
    }

    public static class DeSerializePerson
    {
        public static Person DeSerialize(String Filename)
        {
            String jsonStream = File.ReadAllText(Filename);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            Person jsondeserialized = ser.Deserialize<Person>(jsonStream);
            return jsondeserialized;
        }

    }
}
