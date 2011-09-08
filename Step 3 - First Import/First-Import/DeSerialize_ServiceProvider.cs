using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace First_Import
{
    public class ServiceProvider
    {
        public String alias_list;
        public String created_at;
        public String crunchbase_url;
        public String email_address;
        public String homepage_url;
        public String name;
        public String overview;
        public String permalink;
        public String phone_number;
        public String tag_list;
        public String twitter_username;
        public String updated_at; 
    }


    public static class DeSerializeServiceProvider
    {
        public static ServiceProvider DeSerialize(String Filename)
        {
            
            String jsonStream = File.ReadAllText(Filename);
            jsonStream = new CorrectOutput().output(jsonStream);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            ServiceProvider jsondeserialized = ser.Deserialize<ServiceProvider>(jsonStream);
            return jsondeserialized;
        }

    } 
}
