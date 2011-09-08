/* First Crunchbase Import tool
 * A small tool to create a gql script for the initial import of crunchbase
 * data.
 * 
 * (C) Daniel Kirstenpfad, sones GmbH 2010, http://developers.sones.com  - http://www.sones.com
 * 
 * */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;

namespace First_Import
{
    #region Correct Output
    public class CorrectOutput
    {
        public String output(String input)
        {
            var temp = input.Replace("\v", " ").Replace((char)14, ' ').Replace((char)28, ' ').Replace((char)30, ' ').Replace((char)31, ' ').Replace((char)3, ' ').Replace((char)29, ' ').Replace((char)16, ' ');
            return temp;
        }
    }
    #endregion
    class Program
    {
        static String CorrectEscape(String Input)
        {
            return Input.Replace("\n", "").Replace("\\", "").Replace("\'", "");
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Dictionary<int, string> bufferCompany = new Dictionary<int, string>();
            Dictionary<int, string> bufferFO = new Dictionary<int, string>();
            Console.WriteLine("First Crunchbase Import tool");
            Console.WriteLine();
            Console.WriteLine("A small tool to create a gql script for the initial import of crunchbase");
            Console.WriteLine("data.");
            Console.WriteLine();

            #region Company
            Console.WriteLine("Generating Company GQL Script...");
            using (StreamWriter outfile = new StreamWriter("Step_2_Crunchbase_Company.gql"))
            {
                StreamWriter outfile2 = new StreamWriter("Step_3_Crunchbase_Company-Competitions.gql");
                StreamWriter outfileCompany = new StreamWriter("Company.txt");

                string[] CompanyDirectory = Directory.GetFiles("."+Path.DirectorySeparatorChar+"crunchbase"+Path.DirectorySeparatorChar+"company"+Path.DirectorySeparatorChar, "*.js");

                foreach (string fileName in CompanyDirectory)
                {
                    Company deserialized = null;
                    try
                    {
                        deserialized = DeSerializeCompany.DeSerialize(fileName);


                        StringBuilder sb = new StringBuilder();


                        try
                        {
                            bufferCompany.Add(deserialized.permalink.GetHashCode(), deserialized.permalink);
                            outfileCompany.WriteLine(deserialized.permalink);
   

                            sb.Append("INSERT INTO Company VALUES (");

                            if (deserialized.alias_list != null)
                                sb.Append("Alias_List = \'" + CorrectEscape(deserialized.alias_list) + "\',");

                            if (deserialized.blog_feed_url != null)
                                sb.Append("BlogFeedURL = \'" + CorrectEscape(deserialized.blog_feed_url) + "\',");

                            if (deserialized.blog_url != null)
                                sb.Append("BlogURL = \'" + CorrectEscape(deserialized.blog_url) + "\',");

                            if (deserialized.name != null)
                                sb.Append("Name = \'" + CorrectEscape(deserialized.name) + "\',");

                            if (deserialized.category_code != null)
                                sb.Append("Category = \'" + CorrectEscape(deserialized.category_code) + "\',");

                            if (deserialized.crunchbase_url != null)
                                sb.Append("CrunchbaseURL = \'" + CorrectEscape(deserialized.crunchbase_url) + "\',");

                            if (deserialized.deadpooled_year != null)
                            {
                                if (deserialized.deadpooled_month == null)
                                    deserialized.deadpooled_month = "1";
                                if (deserialized.deadpooled_day == null)
                                    deserialized.deadpooled_day = "1";

                                DateTime deadpooled = new DateTime(Convert.ToInt32(deserialized.deadpooled_year), Convert.ToInt32(deserialized.deadpooled_month), Convert.ToInt32(deserialized.deadpooled_day));
                                sb.Append("Deadpooled_At = \'" + deadpooled + "\',");
                            }

                            if (deserialized.description != null)
                                sb.Append("Description = \'" + CorrectEscape(deserialized.description) + "\',");

                            if (deserialized.number_of_employees != null)
                                sb.Append("NumberOfEmployees = " + CorrectEscape(deserialized.number_of_employees) + ",");

                            if (deserialized.email_address != null)
                                sb.Append("EMailAdress = \'" + CorrectEscape(deserialized.email_address) + "\',");

                            if (deserialized.founded_year != null)
                            {
                                if (deserialized.founded_month == null)
                                    deserialized.founded_month = "1";
                                if (deserialized.founded_day == null)
                                    deserialized.founded_day = "1";

                                DateTime founded = new DateTime(Convert.ToInt32(deserialized.founded_year), Convert.ToInt32(deserialized.founded_month), Convert.ToInt32(deserialized.founded_day));
                                sb.Append("Founded_At = \'" + founded + "\',");
                            }

                            if (deserialized.homepage_url != null)
                                sb.Append("HomepageURL = \'" + CorrectEscape(deserialized.homepage_url) + "\',");

                            if (deserialized.overview != null)
                                sb.Append("Overview = \'" + CorrectEscape(deserialized.overview) + "\',");


                             if (deserialized.permalink != null)
                                sb.Append("Permalink = \'" + CorrectEscape(deserialized.permalink) + "\',");


                            if (deserialized.phone_number != null)
                                sb.Append("PhoneNumber = \'" + CorrectEscape(deserialized.phone_number) + "\',");

                            if (deserialized.tag_list != null)
                                sb.Append("Tags = \'" + CorrectEscape(deserialized.tag_list) + "\',");

                            if (deserialized.twitter_username != null)
                                sb.Append("TwitterUsername = \'" + CorrectEscape(deserialized.twitter_username) + "\',");

                            if (deserialized.updated_at != null)
                            {
                                // Mon, 6 Oct 2003 18:39:47 UTC
                                // ddd, d MMM yyyy hh:mm:s

                                // Wed Dec 23 17:47:41 UTC 2009
                                // ddd MMM d hh:mm:ss UTC yyyy

                                CultureInfo enUS = new CultureInfo("en-US");

                                String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                                //String expectedFormat = "g";
                                DateTime updated_at = DateTime.ParseExact(deserialized.updated_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                                sb.Append("Updated_At = \'" + updated_at.ToString() + "\',");
                            }

                            if (deserialized.created_at != null)
                            {
                                CultureInfo enUS = new CultureInfo("en-US");
                                String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                                DateTime created_at = DateTime.ParseExact(deserialized.created_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                                sb.Append("Created_At = \'" + created_at.ToString() + "\',");
                            }

                            sb.Remove(sb.Length - 1, 1);

                            sb.Append(")");

                            //Console.WriteLine(sb.ToString());

                            outfile.WriteLine(sb.ToString());

                            if (deserialized.Competitions != null)
                            {
                                StringBuilder permalinks = new StringBuilder();

                                foreach (Dictionary<string, Competitor> competitor_dict in deserialized.Competitions)
                                {
                                    foreach (Competitor _competitor in competitor_dict.Values)
                                    {
                                        permalinks.Append("Permalink = \'" + _competitor.permalink + "\',");
                                    }
                                }

                                // remove ending ,
                                if (permalinks.Length > 0)
                                {
                                    permalinks.Remove(permalinks.Length - 1, 1);
                                    outfile2.WriteLine("UPDATE Company SET (Competitions += SETOF(" + permalinks.ToString() + ")) WHERE Permalink = \'" + deserialized.permalink + "\'");
                                }
                            }
                        }


                        catch
                        {
                            Console.WriteLine("Attribute key {0} alredy exist", deserialized.permalink);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("File:" + fileName + " has symbols that are not supported their operating system");
                    }
                }
                    outfile.Close();
                    outfile2.Close();
                    outfileCompany.Close();

                   

            }
            #endregion

            #region FinancialOrganization
            Console.WriteLine("Generating FinancialOrganization GQL Script...");
            using (StreamWriter outfile = new StreamWriter("Step_2_Crunchbase_FinancialOrganization.gql"))
            {
                string[] FinancialOrganizationDirectory = Directory.GetFiles("." + Path.DirectorySeparatorChar + "crunchbase" + Path.DirectorySeparatorChar + "financial-organization" + Path.DirectorySeparatorChar, "*.js");
                StreamWriter outfileFinancialOrganisation = new StreamWriter("FinancialOrganisation.txt");

                foreach (string fileName in FinancialOrganizationDirectory)
                {
                      try{
						FinancialOrganization newFinancialOrganization = DeSerializeFinancialOrganization.DeSerialize(fileName);

                    StringBuilder sb = new StringBuilder();
                           try
                        {
                            bufferFO.Add(newFinancialOrganization.permalink.GetHashCode(), newFinancialOrganization.permalink);
                            outfileFinancialOrganisation.WriteLine(newFinancialOrganization.permalink);
   

                    sb.Append("INSERT INTO FinancialOrganization VALUES (");

                    if (newFinancialOrganization.alias_list != null)
                        sb.Append("Alias_List = \'" + CorrectEscape(newFinancialOrganization.alias_list) + "\',");

                    if (newFinancialOrganization.blog_feed_url != null)
                        sb.Append("BlogFeedURL = \'" + CorrectEscape(newFinancialOrganization.blog_feed_url) + "\',");

                    if (newFinancialOrganization.blog_url != null)
                        sb.Append("BlogURL = \'" + CorrectEscape(newFinancialOrganization.blog_url) + "\',");

                    if (newFinancialOrganization.created_at != null)
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                        DateTime created_at = DateTime.ParseExact(newFinancialOrganization.created_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                        sb.Append("Created_At = \'" + created_at.ToString() + "\',");
                    }

                    if (newFinancialOrganization.crunchbase_url != null)
                        sb.Append("CrunchbaseURL = \'" + CorrectEscape(newFinancialOrganization.crunchbase_url) + "\',");

                    if (newFinancialOrganization.description != null)
                        sb.Append("Description = \'" + CorrectEscape(newFinancialOrganization.description) + "\',");

                    if (newFinancialOrganization.email_address != null)
                        sb.Append("EMailAdress = \'" + CorrectEscape(newFinancialOrganization.email_address) + "\',");

                    if (newFinancialOrganization.founded_year != null)
                    {
                        if (newFinancialOrganization.founded_month == null)
                            newFinancialOrganization.founded_month = "1";
                        if (newFinancialOrganization.founded_day == null)
                            newFinancialOrganization.founded_day = "1";

                        DateTime founded = new DateTime(Convert.ToInt32(newFinancialOrganization.founded_year), Convert.ToInt32(newFinancialOrganization.founded_month), Convert.ToInt32(newFinancialOrganization.founded_day));
                        sb.Append("Founded_At = \'" + founded + "\',");
                    }

                    if (newFinancialOrganization.homepage_url != null)
                        sb.Append("HomepageURL = \'" + CorrectEscape(newFinancialOrganization.homepage_url) + "\',");

                    if (newFinancialOrganization.name != null)
                        sb.Append("Name = \'" + CorrectEscape(newFinancialOrganization.name) + "\',");

                    if (newFinancialOrganization.number_of_employees != null)
                        sb.Append("NumberOfEmployees = " + CorrectEscape(newFinancialOrganization.number_of_employees) + ",");

                    if (newFinancialOrganization.overview != null)
                        sb.Append("Overview = \'" + CorrectEscape(newFinancialOrganization.overview) + "\',");

                   
                    if (newFinancialOrganization.permalink != null)
                        sb.Append("Permalink = \'" + CorrectEscape(newFinancialOrganization.permalink) + "\',");
                   
                    if (newFinancialOrganization.phone_number != null)
                        sb.Append("PhoneNumber = \'" + CorrectEscape(newFinancialOrganization.phone_number) + "\',");

                    if (newFinancialOrganization.tag_list != null)
                        sb.Append("Tags = \'" + CorrectEscape(newFinancialOrganization.tag_list) + "\',");

                    if (newFinancialOrganization.twitter_username != null)
                        sb.Append("TwitterUsername = \'" + CorrectEscape(newFinancialOrganization.twitter_username) + "\',");

                    if (newFinancialOrganization.updated_at != null)
                    {
                        // Mon, 6 Oct 2003 18:39:47 UTC
                        // ddd, d MMM yyyy hh:mm:s

                        // Wed Dec 23 17:47:41 UTC 2009
                        // ddd MMM d hh:mm:ss UTC yyyy

                        CultureInfo enUS = new CultureInfo("en-US");

                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                        //String expectedFormat = "g";
                        DateTime updated_at = DateTime.ParseExact(newFinancialOrganization.updated_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                        sb.Append("Updated_At = \'" + updated_at.ToString() + "\',");
                    }

                    sb.Remove(sb.Length - 1, 1);

                    sb.Append(")");

                    //Console.WriteLine(sb.ToString());

                    outfile.WriteLine(sb.ToString());
                }
                           catch
                           {
                               Console.WriteLine("Attribute key {0} alredy exist", newFinancialOrganization.permalink);
                           }
                      }
						catch (Exception e)
					{
						Console.WriteLine("File:"+fileName+" has symbols that are not supported their operating system");
					}}
                outfile.Close();
                bufferFO.Clear();
                outfileFinancialOrganisation.Close();
            }
            #endregion

            #region Product
            Console.WriteLine("Generating Product GQL Script...");
            using (StreamWriter outfile = new StreamWriter("Step_2_Crunchbase_Product.gql"))
            {
                string[] CompanyDirectory = Directory.GetFiles("." + Path.DirectorySeparatorChar + "crunchbase" + Path.DirectorySeparatorChar + "product" + Path.DirectorySeparatorChar, "*.js");
                StreamWriter outfileProduct = new StreamWriter("Product.txt");

                foreach (string fileName in CompanyDirectory)
                {
                    try
                    {
                        Product deserialized = DeSerializeProduct.DeSerialize(fileName);

                        StringBuilder sb = new StringBuilder();

                        try
                        {

                            if (bufferCompany.ContainsValue(deserialized.company.permalink))
                            {

                                try
                                {
                                    bufferFO.Add(deserialized.permalink.GetHashCode(), deserialized.permalink);
                                    outfileProduct.WriteLine(deserialized.permalink);


                                    sb.Append("INSERT INTO Product VALUES (");

                                    if (deserialized.blog_feed_url != null)
                                        sb.Append("BlogFeedURL = \'" + CorrectEscape(deserialized.blog_feed_url) + "\',");

                                    if (deserialized.blog_url != null)
                                        sb.Append("BlogURL = \'" + CorrectEscape(deserialized.blog_url) + "\',");

                                    if (deserialized.created_at != null)
                                    {
                                        CultureInfo enUS = new CultureInfo("en-US");
                                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                                        DateTime created_at = DateTime.ParseExact(deserialized.created_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                                        sb.Append("Created_At = \'" + created_at.ToString() + "\',");
                                    }

                                    if (deserialized.deadpooled_year != null)
                                    {
                                        if (deserialized.deadpooled_month == null)
                                            deserialized.deadpooled_month = "1";
                                        if (deserialized.deadpooled_day == null)
                                            deserialized.deadpooled_day = "1";

                                        DateTime deadpooled = new DateTime(Convert.ToInt32(deserialized.deadpooled_year), Convert.ToInt32(deserialized.deadpooled_month), Convert.ToInt32(deserialized.deadpooled_day));
                                        sb.Append("Deadpooled_At = \'" + deadpooled + "\',");
                                    }

                                    if (deserialized.homepage_url != null)
                                        sb.Append("HomepageURL = \'" + CorrectEscape(deserialized.homepage_url) + "\',");

                                    if (deserialized.invite_share_url != null)
                                        sb.Append("InviteShareURL = \'" + CorrectEscape(deserialized.invite_share_url) + "\',");

                                    if (deserialized.launched_year != null)
                                    {
                                        if (deserialized.launched_month == null)
                                            deserialized.launched_month = "1";
                                        if (deserialized.launched_day == null)
                                            deserialized.launched_day = "1";

                                        DateTime launched_at = new DateTime(Convert.ToInt32(deserialized.launched_year), Convert.ToInt32(deserialized.launched_month), Convert.ToInt32(deserialized.launched_day));
                                        sb.Append("Launched_At = \'" + launched_at + "\',");
                                    }

                                    if (deserialized.name != null)
                                        sb.Append("Name = \'" + CorrectEscape(deserialized.name) + "\',");

                                    if (deserialized.overview != null)
                                        sb.Append("Overview = \'" + CorrectEscape(deserialized.overview) + "\',");

                                    if (deserialized.permalink != null)
                                        sb.Append("Permalink = \'" + CorrectEscape(deserialized.permalink) + "\',");

                                    if (deserialized.stage_code != null)
                                        sb.Append("StageCode = \'" + CorrectEscape(deserialized.stage_code) + "\',");

                                    if (deserialized.tag_list != null)
                                        sb.Append("Tags = \'" + CorrectEscape(deserialized.tag_list) + "\',");

                                    if (deserialized.twitter_username != null)
                                        sb.Append("TwitterUsername = \'" + CorrectEscape(deserialized.twitter_username) + "\',");

                                    if (deserialized.company != null)
                                    {
                                        if (deserialized.company.permalink != null)
                                        {
                                            sb.Append("Company = REF(Permalink = \'" + CorrectEscape(deserialized.company.permalink) + "\'),");
                                        }
                                    }

                                    if (deserialized.updated_at != null)
                                    {
                                        // Mon, 6 Oct 2003 18:39:47 UTC
                                        // ddd, d MMM yyyy hh:mm:s

                                        // Wed Dec 23 17:47:41 UTC 2009
                                        // ddd MMM d hh:mm:ss UTC yyyy

                                        CultureInfo enUS = new CultureInfo("en-US");

                                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                                        //String expectedFormat = "g";
                                        DateTime updated_at = DateTime.ParseExact(deserialized.updated_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                                        sb.Append("Updated_At = \'" + updated_at.ToString() + "\',");
                                    }

                                    sb.Remove(sb.Length - 1, 1);

                                    sb.Append(")");

                                    //Console.WriteLine(sb.ToString());

                                    outfile.WriteLine(sb.ToString());
                                }
                                catch
                                {
                                    Console.WriteLine("Attribute key {0} alredy exist", deserialized.permalink);
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Product {0} has incorrect link to Company or is null",deserialized.permalink);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("File:" + fileName + " has symbols that are not supported their operating system");
                    }
                }
                outfile.Close();
                bufferFO.Clear();
                outfileProduct.Close();
            }
            #endregion

            #region Person
            Console.WriteLine("Generating Person GQL Script...");
            using (StreamWriter outfile = new StreamWriter("Step_2_Crunchbase_Person.gql"))
            {
                string[] CompanyDirectory = Directory.GetFiles("." + Path.DirectorySeparatorChar + "crunchbase" + Path.DirectorySeparatorChar + "person" + Path.DirectorySeparatorChar, "*.js");
                StreamWriter outfilePerson = new StreamWriter("Person.txt");

                foreach (string fileName in CompanyDirectory)
                {
					try{
                    Person deserialized = DeSerializePerson.DeSerialize(fileName);

                    StringBuilder sb = new StringBuilder();

                        
                        try
                           {
                                bufferFO.Add(deserialized.permalink.GetHashCode(), deserialized.permalink);
                                outfilePerson.WriteLine(deserialized.permalink);

                    sb.Append("INSERT INTO Person VALUES (");


                    if (deserialized.affiliation_name != null)
                        sb.Append("AffiliationName = \'" + CorrectEscape(deserialized.affiliation_name) + "\',");

                    if (deserialized.alias_list != null)
                        sb.Append("Alias_List = \'" + CorrectEscape(deserialized.alias_list) + "\',");

                    if (deserialized.birthplace != null)
                        sb.Append("Birthplace = \'" + CorrectEscape(deserialized.birthplace) + "\',");

                    if (deserialized.blog_feed_url != null)
                        sb.Append("BlogFeedURL = \'" + CorrectEscape(deserialized.blog_feed_url) + "\',");

                    if (deserialized.blog_url != null)
                        sb.Append("BlogURL = \'" + CorrectEscape(deserialized.blog_url) + "\',");

                    if (deserialized.born_year != null)
                    {
                        if (deserialized.born_month == null)
                            deserialized.born_month = "1";
                        if (deserialized.born_day == null)
                            deserialized.born_day = "1";

                        DateTime deadpooled = new DateTime(Convert.ToInt32(deserialized.born_year), Convert.ToInt32(deserialized.born_month), Convert.ToInt32(deserialized.born_day));
                        sb.Append("Birthday = \'" + deadpooled + "\',");
                    }

                    if (deserialized.created_at != null)
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                        DateTime created_at = DateTime.ParseExact(deserialized.created_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                        sb.Append("Created_At = \'" + created_at.ToString() + "\',");
                    }

                    if (deserialized.crunchbase_url != null)
                        sb.Append("CrunchbaseURL = \'" + CorrectEscape(deserialized.crunchbase_url) + "\',");

                    if (deserialized.first_name != null)
                        sb.Append("FirstName = \'" + CorrectEscape(deserialized.first_name) + "\',");

                    if (deserialized.homepage_url != null)
                        sb.Append("HomepageURL = \'" + CorrectEscape(deserialized.homepage_url) + "\',");

                    if (deserialized.last_name != null)
                        sb.Append("LastName = \'" + CorrectEscape(deserialized.last_name) + "\',");

                    if (deserialized.overview != null)
                        sb.Append("Overview = \'" + CorrectEscape(deserialized.overview) + "\',");

                    if (deserialized.permalink != null)
                        sb.Append("Permalink = \'" + CorrectEscape(deserialized.permalink) + "\',");

                    if (deserialized.tag_list != null)
                        sb.Append("Tags = \'" + CorrectEscape(deserialized.tag_list) + "\',");

                    if (deserialized.twitter_username != null)
                        sb.Append("TwitterUsername = \'" + CorrectEscape(deserialized.twitter_username) + "\',");

                    if (deserialized.updated_at != null)
                    {
                        // Mon, 6 Oct 2003 18:39:47 UTC
                        // ddd, d MMM yyyy hh:mm:s

                        // Wed Dec 23 17:47:41 UTC 2009
                        // ddd MMM d hh:mm:ss UTC yyyy

                        CultureInfo enUS = new CultureInfo("en-US");

                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                        //String expectedFormat = "g";
                        DateTime updated_at = DateTime.ParseExact(deserialized.updated_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                        sb.Append("Updated_At = \'" + updated_at.ToString() + "\',");
                    }



                    sb.Remove(sb.Length - 1, 1);

                    sb.Append(")");

                    //Console.WriteLine(sb.ToString());

                    outfile.WriteLine(sb.ToString());
                }
                        catch
                        {
                            Console.WriteLine("Attribute key {0} alredy exist", deserialized.permalink);
                        }
                    }
									catch (Exception e)
					{
						Console.WriteLine("File:"+fileName+" has symbols that are not supported their operating system");
					}}

                outfile.Close();
                bufferFO.Clear();
                outfilePerson.Close();
            }
            #endregion

            #region ServiceProvider
            Console.WriteLine("Generating ServiceProvider GQL Script...");
            using (StreamWriter outfile = new StreamWriter("Step_2_Crunchbase_ServiceProvider.gql"))
            {
                string[] CompanyDirectory = Directory.GetFiles("." + Path.DirectorySeparatorChar + "crunchbase" + Path.DirectorySeparatorChar + "service-provider" + Path.DirectorySeparatorChar, "*.js");
                StreamWriter outfileService = new StreamWriter("ServiceProvider.txt");

                foreach (string fileName in CompanyDirectory)
                {
                    try
					{ServiceProvider deserialized = DeSerializeServiceProvider.DeSerialize(fileName);

                    StringBuilder sb = new StringBuilder();

                    try
                    {
                        bufferFO.Add(deserialized.permalink.GetHashCode(), deserialized.permalink);
                        outfileService.WriteLine(deserialized.permalink);

                    sb.Append("INSERT INTO ServiceProvider VALUES (");


                    if (deserialized.alias_list != null)
                        sb.Append("Alias_List = \'" + CorrectEscape(deserialized.alias_list) + "\',");

                    if (deserialized.created_at != null)
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                        DateTime created_at = DateTime.ParseExact(deserialized.created_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                        sb.Append("Created_At = \'" + created_at.ToString() + "\',");
                    }

                    if (deserialized.crunchbase_url != null)
                        sb.Append("CrunchbaseURL = \'" + CorrectEscape(deserialized.crunchbase_url) + "\',");

                    if (deserialized.email_address != null)
                        sb.Append("EMailAdress = \'" + CorrectEscape(deserialized.email_address) + "\',");

                    if (deserialized.homepage_url != null)
                        sb.Append("HomepageURL = \'" + CorrectEscape(deserialized.homepage_url) + "\',");

                    if (deserialized.name != null)
                        sb.Append("Name = \'" + CorrectEscape(deserialized.name) + "\',");

                    if (deserialized.overview != null)
                        sb.Append("Overview = \'" + CorrectEscape(deserialized.overview) + "\',");

                    if (deserialized.permalink != null)
                        sb.Append("Permalink = \'" + CorrectEscape(deserialized.permalink) + "\',");

                    if (deserialized.phone_number != null)
                        sb.Append("PhoneNumber = \'" + CorrectEscape(deserialized.phone_number) + "\',");

                    if (deserialized.tag_list != null)
                        sb.Append("Tags = \'" + CorrectEscape(deserialized.tag_list) + "\',");

                    if (deserialized.updated_at != null)
                    {
                        // Mon, 6 Oct 2003 18:39:47 UTC
                        // ddd, d MMM yyyy hh:mm:s

                        // Wed Dec 23 17:47:41 UTC 2009
                        // ddd MMM d hh:mm:ss UTC yyyy

                        CultureInfo enUS = new CultureInfo("en-US");

                        String expectedFormat = "ddd MMM d H:mm:ss yyyy";
                        //String expectedFormat = "g";
                        DateTime updated_at = DateTime.ParseExact(deserialized.updated_at.Replace("UTC ", ""), expectedFormat, enUS, DateTimeStyles.AssumeUniversal);

                        sb.Append("Updated_At = \'" + updated_at.ToString() + "\',");
                    }

                    sb.Remove(sb.Length - 1, 1);

                    sb.Append(")");

                    //Console.WriteLine(sb.ToString());

                    outfile.WriteLine(sb.ToString());
                }
                  catch
                       {
                           Console.WriteLine("Attribute key {0} alredy exist", deserialized.permalink);
                       }
              }
					catch (Exception e)
					{
						Console.WriteLine("File:"+fileName+" has symbols that are not supported their operating system");
					}}
                outfile.Close();
                bufferFO.Clear();
                bufferCompany.Clear();
                outfileService.Close();
            }
            #endregion

        }
    }
}