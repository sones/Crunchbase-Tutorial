/* MirrorCrunchbaseJSON tool
 * A small tool to mirror the CrunchBase JSON database API results
 * to the local disk for further research.
 * 
 * Partly based and inspired by http://crunchbasegrabber.codeplex.com/
 * 
 * Distribute freely under Microsoft Public License (Ms-PL) (http://crunchbasegrabber.codeplex.com/license)
 * 
 * (C) Daniel Kirstenpfad, sones GmbH 2010, http://developers.sones.com  - http://www.sones.com
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Threading;

namespace CrunchbaseGrabber
{
    class Program
    {
        #region GetJsonStream
        public static string GetJsonStream(string URLBase, string serviceprovidername)
        {
            string jsonStream;
            string urlEnd = ".js";

            WebRequest wrGetURL;
            wrGetURL = WebRequest.Create(URLBase + serviceprovidername + urlEnd);

            try
            {
                jsonStream = new StreamReader(wrGetURL.GetResponse().GetResponseStream()).ReadToEnd();
                return jsonStream;
            }
            catch (System.Net.WebException e)
            {
                Console.Write("x ({0})", serviceprovidername);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception Caught: {0}", e.ToString());
            }

            return jsonStream = null;
        }
        #endregion


        static void Main(string[] args)
        {
            #region Helper Instances
            bool override_file = false;

            ProductsGenerator productsGenerator = new ProductsGenerator();
            CompanyGenerator companyGenerator = new CompanyGenerator();
            PeopleGenerator peopleGenerator = new PeopleGenerator();
            FinancialGenerator financialGenerator = new FinancialGenerator();
            ServiceProviderGenerator serviceprovidergenerator = new ServiceProviderGenerator();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            WebRequest wrGetURL;
            string jsonStream;
            #endregion

            if (!Directory.Exists("crunchbase"))
                Directory.CreateDirectory("crunchbase");
            
            #region Companies
            Console.Write("Getting all company names...");
            wrGetURL = WebRequest.Create("http://api.crunchbase.com/v/1/companies.js");
            String companies = new StreamReader(wrGetURL.GetResponse().GetResponseStream()).ReadToEnd();
            StreamWriter file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "companies.js");
                file.Write(companies);
            file.Close();
            Console.WriteLine("done");

            Console.Write("Parsing company names...");
                List<cbCompanyObject> ParsedCompanies =  companyGenerator.GetCompanyNames(companies);
            Console.WriteLine("done");

            Console.Write("Getting each company info...");            
            if (!Directory.Exists("crunchbase"+Path.DirectorySeparatorChar+"company"))
                Directory.CreateDirectory("crunchbase"+Path.DirectorySeparatorChar+"company");
            foreach (cbCompanyObject company in ParsedCompanies)
            {
                #region Get the JSON Results for every company and output it to a file...
                string jsonLine;

                //with a company name parsed from JSON, create the stream of the company specific JSON
                jsonStream = GetJsonStream("http://api.crunchbase.com/v/1/company/",company.permalink);

                string filename = company.permalink.Replace(Path.DirectorySeparatorChar, ' ');

                if (jsonStream != null)
                {
                    file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "company" + Path.DirectorySeparatorChar + filename + ".js");
                    file.Write(jsonStream);
                    file.Close();
                }

                Console.Write(".");
                Thread.Sleep(500);
                #endregion
            }
            Console.WriteLine("done");
            #endregion

            #region people

            Console.Write("Getting all peoples names...");
            wrGetURL = WebRequest.Create("http://api.crunchbase.com/v/1/people.js");
            String people = new StreamReader(wrGetURL.GetResponse().GetResponseStream()).ReadToEnd();
            file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "people.js");
            file.Write(people);
            file.Close();
            Console.WriteLine("done");


            Console.Write("Parsing peoples names...");
             List<cbPeoplesObject> parsedPeoples =  peopleGenerator.GetPeoples(people);
            Console.WriteLine("done");

            Console.Write("Getting each persons info...");
            //string jsonStream;

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "person"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "person");

            foreach (cbPeoplesObject person in parsedPeoples)
            {
                string jsonLine;

                //with a company name parsed from JSON, create the stream of the company specific JSON
                jsonStream = GetJsonStream("http://api.crunchbase.com/v/1/person/", person.permalink);

                string filename = person.permalink.Replace(Path.DirectorySeparatorChar, ' ');

                if (jsonStream != null)
                {
                    file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "person" + Path.DirectorySeparatorChar + filename + ".js");
                    file.Write(jsonStream);
                    file.Close();
                }

                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.WriteLine("done");

            #endregion

            #region products

            Console.Write("Getting all products names...");
            wrGetURL = WebRequest.Create("http://api.crunchbase.com/v/1/products.js");
            String products = new StreamReader(wrGetURL.GetResponse().GetResponseStream()).ReadToEnd();
            
            file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "products.js");
            file.Write(products);
            file.Close();
            Console.WriteLine("done");


            Console.Write("Parsing products names...");
            List<cbProductsObject> parsedProducts = productsGenerator.GetProducts(products);
            Console.WriteLine("done");

            Console.Write("Getting each products info...");

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "product"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "product");

            foreach (cbProductsObject product in parsedProducts)
            {
                string jsonLine;

                jsonStream = GetJsonStream("http://api.crunchbase.com/v/1/product/",product.permalink);

                string filename = product.permalink.Replace(Path.DirectorySeparatorChar, ' ');

                if (jsonStream != null)
                {
                    file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "product" + Path.DirectorySeparatorChar + filename + ".js");
                    file.Write(jsonStream);
                    file.Close();
                }
                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.WriteLine("done");

            #endregion

            #region financial-organizations

            Console.Write("Getting all financial-organizations names...");
            wrGetURL = WebRequest.Create("http://api.crunchbase.com/v/1/financial-organizations.js");
            String financialorganizations = new StreamReader(wrGetURL.GetResponse().GetResponseStream()).ReadToEnd();
            file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "financial-organizations.js");
            file.Write(financialorganizations);
            file.Close();
            Console.WriteLine("done");

            Console.Write("Parsing financial-organizations names...");
            List<cbFinancialObject> parsedFinancial = financialGenerator.GetFinancials(financialorganizations);
            Console.WriteLine("done");

            Console.Write("Getting each financial-organization info...");

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "financial-organization"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "financial-organization");

            foreach (cbFinancialObject financial in parsedFinancial)
            {
                string jsonLine;

                jsonStream = GetJsonStream("http://api.crunchbase.com/v/1/financial-organization/", financial.permalink);

                string filename = financial.permalink.Replace(Path.DirectorySeparatorChar, ' ');

                if (jsonStream != null)
                {
                    file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "financial-organization" + Path.DirectorySeparatorChar + filename + ".js");
                    file.Write(jsonStream);
                    file.Close();
                }
                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.WriteLine("done");

            #endregion

            #region service-providers

            Console.Write("Getting all service-provider names...");
            wrGetURL = WebRequest.Create("http://api.crunchbase.com/v/1/service-providers.js");
            String serviceproviders = new StreamReader(wrGetURL.GetResponse().GetResponseStream()).ReadToEnd();
            file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "service-providers.js");
            file.Write(serviceproviders);
            file.Close();
            Console.WriteLine("done");

            Console.Write("Parsing service-provider names...");
            List<cbServiceproviderObject> parsedServiceProviders = serviceprovidergenerator.GetFinancials(serviceproviders);
            Console.WriteLine("done");

            Console.Write("Getting each service-provider info...");

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "service-provider"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "service-provider");

            foreach (cbServiceproviderObject servicep in parsedServiceProviders)
            {
                string jsonLine;

                jsonStream = GetJsonStream("http://api.crunchbase.com/v/1/service-provider/",servicep.permalink);

                string filename = servicep.permalink.Replace(Path.DirectorySeparatorChar, ' ');

                if (jsonStream != null)
                {
                    file = new StreamWriter("crunchbase" + Path.DirectorySeparatorChar + "service-provider" + Path.DirectorySeparatorChar + filename + ".js");
                    file.Write(jsonStream);
                    file.Close();
                }
                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.WriteLine("done");

            #endregion

        }
    }

    #region Helpers

    public class CompanyGenerator
    {
        //this is how we call out to crunchbase to get their full list of companies
        public List<cbCompanyObject> GetCompanyNames(String jsonStream)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            List<cbCompanyObject> jsonCompanies = ser.Deserialize<List<cbCompanyObject>>(jsonStream);
            return jsonCompanies;
        }

    }

    public class CompanyJsonObject
    {
        public List<cbCompanyObject> cbcompanyObj;
    }

    //the simple definition for the deserialized object
    public class cbCompanyObject
    {
        public string name;
        public string permalink;
    }

    public class PeopleGenerator
    {
        //this is how we call out to crunchbase to get their full list of companies
        public List<cbPeoplesObject> GetPeoples(String jsonStream)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            List<cbPeoplesObject> jsonCompanies = ser.Deserialize<List<cbPeoplesObject>>(jsonStream);
            return jsonCompanies;
        }

    }
    public class PeopleJsonObject
    {
        public List<cbPeoplesObject> cbPeoplesObj;
    }

    //the simple definition for the deserialized object
    public class cbPeoplesObject
    {
        public string first_name;
        public string last_name;
        public string permalink;
    }

    public class ProductsGenerator
    {
        //this is how we call out to crunchbase to get their full list of companies
        public List<cbProductsObject> GetProducts(String jsonStream)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            List<cbProductsObject> jsonProducts = ser.Deserialize<List<cbProductsObject>>(jsonStream);
            return jsonProducts;
        }

    }
    public class ProductsJsonObject
    {
        public List<cbProductsObject> cbProductsObj;
    }

    //the simple definition for the deserialized object
    public class cbProductsObject
    {
        public string name;
        public string permalink;
    }

    public class FinancialGenerator
    {
        public List<cbFinancialObject> GetFinancials(String jsonStream)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            List<cbFinancialObject> jsonFinancial = ser.Deserialize<List<cbFinancialObject>>(jsonStream);
            return jsonFinancial;
        }

    }
    public class FinancialJsonObject
    {
        public List<cbFinancialObject> cbFinancialObj;
    }

    //the simple definition for the deserialized object
    public class cbFinancialObject
    {
        public string name;
        public string permalink;
    }

    public class ServiceProviderGenerator
    {
        public List<cbServiceproviderObject> GetFinancials(String jsonStream)
        {
            System.Web.Script.Serialization.JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.MaxJsonLength = 20000000;
            //stick it into a list
            List<cbServiceproviderObject> jsonFinancial = ser.Deserialize<List<cbServiceproviderObject>>(jsonStream);
            return jsonFinancial;
        }

    }
    public class ServiceProviderJsonObject
    {
        public List<cbServiceproviderObject> cbServiceProviderObj;
    }

    //the simple definition for the deserialized object
    public class cbServiceproviderObject
    {
        public string name;
        public string permalink;
    }

    #endregion
}
