using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace First_Import
{

    public class Product
    {
        public String blog_feed_url;
        public String blog_url; 
        public String created_at; 
        public String crunchbase_url;
        public String deadpooled_year;
        public String deadpooled_month;
        public String deadpooled_day;
        public String homepage_url;
        public String invite_share_url;
        public String launched_year;
        public String launched_month;
        public String launched_day;
        public String name;
        public String overview; 
        public String permalink; 
        public String stage_code; 
        public String tag_list; 
        public String twitter_username;
        public String updated_at;
        public NameAndPermalink company;
    }

    public static class DeSerializeProduct
    {
        public static Product DeSerialize(String Filename)
        {
            String jsonStream = File.ReadAllText(Filename);
            jsonStream = new CorrectOutput().output(jsonStream);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            Product jsondeserialized = ser.Deserialize<Product>(jsonStream);
            return jsondeserialized;
        }

    }
}
