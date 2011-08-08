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
using System.Security.Cryptography;

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
                Console.Write("x ({0} ) \n\r Exception:{1}", serviceprovidername, e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception Caught: {0}", e.ToString());
            }

            return jsonStream = null;
        }
        #endregion
        #region GetInfo
        public class DownloadChildFileTask 
        {
            public String sWebPath { get; set; }
            public String slink { get; set; }
            public String sFilePathDiff { get; set; }

        }

        public class ThreadPara
        {
            public ManualResetEvent m_Event;
            public int num;
        }

        public class DownloadFile
        {
            #region Variable

            //The current need to download the list of tasks,
            //completed clear it, 
            //the number of tasks to determine how many threads start
            public List<DownloadChildFileTask> cdChildInfoTasks = new List<DownloadChildFileTask>(22);


            //Record download status
            String m_ProgressPrefix = "";

            //Record lastest download status
            String m_LastPrefix = null;

            //Record lastest download file Num
            long m_LastFileNum = 0;



            #endregion

            #region RunTask
            
            //Run download task,
            public int RunChildFileDownloadTask()
            {
                int CurrTaskNum = 0;

                //This is used to mark the end of ThreadTask
                ManualResetEvent[] manualEventArray = new ManualResetEvent[cdChildInfoTasks.Count];

                //get task from cdChildInfoTasks
                for (CurrTaskNum = 0; CurrTaskNum < cdChildInfoTasks.Count; CurrTaskNum++)
                {

                    manualEventArray[CurrTaskNum] = new ManualResetEvent(false);
                    ThreadPara para = new ThreadPara() { m_Event = manualEventArray[CurrTaskNum], num = CurrTaskNum };
                    ThreadPool.QueueUserWorkItem(RunAChildTask, para);   

                }

                //wait for all task done
                WaitHandle.WaitAll(manualEventArray);
                
                //clear the task list
                cdChildInfoTasks.Clear();

                return CurrTaskNum;
            }

            //Thread Method
            public void RunAChildTask(object obj)
            {
                int num = ((ThreadPara)obj).num;

                DownloadChildFile( cdChildInfoTasks[num].sWebPath,cdChildInfoTasks[num].slink,cdChildInfoTasks[num].sFilePathDiff);

                ((ThreadPara)obj).m_Event.Set();
            }
            #endregion

            public static string GetMD5Hash(string TextToHash)
            {
                //Prüfen ob Daten übergeben wurden.
                if ((TextToHash == null) || (TextToHash.Length == 0))
                {
                    return string.Empty;
                }

                //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
                //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] textToHash = Encoding.Default.GetBytes(TextToHash);
                byte[] result = md5.ComputeHash(textToHash);

                return System.BitConverter.ToString(result);
            }

            #region DownloadParentFileAndGetParentInfo
            //this is how we call out to crunchbase to get their full string of Children
            //download parent file like companies.js/people.js
            public String DownloadParentFileAndGetParentInfo(String sWebPath,  String sParentName,String sSaveFilePath)
            {
                Console.Write("Getting all " + sParentName + " names...");

                String parents;

                if (File.Exists(sSaveFilePath))
                {
                    parents = new StreamReader(sSaveFilePath).ReadToEnd();
                }
                else
                {
                    WebRequest wrGetURL = WebRequest.Create(sWebPath);
                    parents = new StreamReader(wrGetURL.GetResponse().GetResponseStream()).ReadToEnd();
                    StreamWriter file = new StreamWriter(sSaveFilePath);
                    file.Write(parents);
                    file.Close();
                }
                Console.WriteLine("done");
                return parents;
            }
            #endregion

            #region DownloadChildFile
            //download child file
            public void DownloadChildFile(String sWebPath, String slink,  String sFilePathDiff)
            {

                string filename = slink.Replace(Path.DirectorySeparatorChar, ' ');
                string sChildPath = "crunchbase" + Path.DirectorySeparatorChar + sFilePathDiff + Path.DirectorySeparatorChar + GetMD5Hash(filename) + ".js";

                if (!File.Exists(sChildPath))
                {
                    String jsonStream = GetJsonStream(sWebPath, slink);
                    if (jsonStream != null)
                    {
                        StreamWriter file = new StreamWriter(sChildPath);
                        file.Write(jsonStream);
                        file.Close();
                    }
                }
        
                Console.Write(".");
               // Thread.Sleep(500);
            }
            #endregion

            #region ShowDownloadProgress
            //Display the download progress
            public void ShowDownloadProgress(long CurrNum, long AllNum, String CurrPrefix)
            {
                //display is updated every 100 files completed
                if (CurrNum == AllNum||CurrNum % 100 == 0)
                {
                    if (m_LastPrefix == null)
                    {
                        m_LastPrefix = CurrPrefix;
                        m_LastFileNum = AllNum;
                    }

                    if (m_LastPrefix != CurrPrefix)
                    {
                        m_ProgressPrefix = m_ProgressPrefix +  m_LastPrefix + "   100%  total:"+ m_LastFileNum.ToString() +"\n\r";
                        m_LastPrefix = CurrPrefix;
                        m_LastFileNum = AllNum;
                    }
                    Console.Clear();
                    double progress =(double) CurrNum/AllNum;

                    Console.Write(m_ProgressPrefix + CurrPrefix + "   " + progress.ToString("P") + "   " + CurrNum.ToString()+ "/" + AllNum.ToString() + "\n\r");
                }
            }
            #endregion

        }

        #endregion

        static void Main(string[] args)
        {
            #region Helper Instances

            ProductsGenerator productsGenerator = new ProductsGenerator();
            CompanyGenerator companyGenerator = new CompanyGenerator();
            PeopleGenerator peopleGenerator = new PeopleGenerator();
            FinancialGenerator financialGenerator = new FinancialGenerator();
            ServiceProviderGenerator serviceprovidergenerator = new ServiceProviderGenerator();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DownloadFile myDownload = new DownloadFile();
            int num = 0;
            int AllNum = 0;
            int ThreadNum = 20;
            #endregion

            if (!Directory.Exists("crunchbase"))
                Directory.CreateDirectory("crunchbase");
            
            #region Companies

            
            String companies = myDownload.DownloadParentFileAndGetParentInfo("http://api.crunchbase.com/v/1/companies.js", "company", "crunchbase" + Path.DirectorySeparatorChar + "companies.js");

            Console.Write("Parsing company names...");
                List<cbCompanyObject> ParsedCompanies =  companyGenerator.GetCompanyNames(companies);
            Console.WriteLine("done");

            Console.Write("Getting each company info...");            
            if (!Directory.Exists("crunchbase"+Path.DirectorySeparatorChar+"company"))
                Directory.CreateDirectory("crunchbase"+Path.DirectorySeparatorChar+"company");

            num = 0;
            AllNum = ParsedCompanies.Count;
            foreach (cbCompanyObject company in ParsedCompanies)
            {
                #region Get the JSON Results for every company and output it to a file...

                //create download task
                DownloadChildFileTask atask = new DownloadChildFileTask() { sFilePathDiff = "company", slink = company.permalink, sWebPath = "http://api.crunchbase.com/v/1/company/" };
                myDownload.cdChildInfoTasks.Add(atask);

                
                if (num % ThreadNum == 0 || num == AllNum)
                {
                    myDownload.RunChildFileDownloadTask();
                }

                //myDownload.DownloadChildFile("http://api.crunchbase.com/v/1/company/", company.permalink, "company");

                num++;
                myDownload.ShowDownloadProgress(num, AllNum, "DownLoad companies:");
                
                #endregion
            }
            Console.WriteLine("done");
            #endregion

            #region people

            String people = myDownload.DownloadParentFileAndGetParentInfo("http://api.crunchbase.com/v/1/people.js", "peoples", "crunchbase" + Path.DirectorySeparatorChar + "people.js");

            Console.Write("Parsing peoples names...");
             List<cbPeoplesObject> parsedPeoples =  peopleGenerator.GetPeoples(people);
            Console.WriteLine("done");

            Console.Write("Getting each persons info...");
            //string jsonStream;

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "person"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "person");

            num = 0;
            AllNum = parsedPeoples.Count;
            foreach (cbPeoplesObject person in parsedPeoples)
            {
                DownloadChildFileTask atask = new DownloadChildFileTask(){ sFilePathDiff = "person",slink = person.permalink, sWebPath = "http://api.crunchbase.com/v/1/person/"};
                myDownload.cdChildInfoTasks.Add(atask);
                if (num % ThreadNum == 0 || num == AllNum)
                {
                    myDownload.RunChildFileDownloadTask();
                }

                num++;
                myDownload.ShowDownloadProgress(num, AllNum, "DownLoad persons:");

            }
            Console.WriteLine("done");

            #endregion

            #region products

            String products = myDownload.DownloadParentFileAndGetParentInfo("http://api.crunchbase.com/v/1/products.js", "products", "crunchbase" + Path.DirectorySeparatorChar + "products.js");


            Console.Write("Parsing products names...");
            List<cbProductsObject> parsedProducts = productsGenerator.GetProducts(products);
            Console.WriteLine("done");

            Console.Write("Getting each products info...");

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "product"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "product");

            num = 0;
            AllNum = parsedProducts.Count;
            foreach (cbProductsObject product in parsedProducts)
            {

                DownloadChildFileTask atask = new DownloadChildFileTask() { sFilePathDiff = "product", slink = product.permalink, sWebPath = "http://api.crunchbase.com/v/1/product/" };
                myDownload.cdChildInfoTasks.Add(atask);

                if (num % ThreadNum == 0 || num == AllNum)
                {
                    myDownload.RunChildFileDownloadTask();
                }

                //myDownload.DownloadChildFile("http://api.crunchbase.com/v/1/product/", product.permalink, "product");

                num++;
                myDownload.ShowDownloadProgress(num, AllNum, "DownLoad products:");
            }
            Console.WriteLine("done");

            #endregion

            #region financial-organizations

            String financialorganizations = myDownload.DownloadParentFileAndGetParentInfo("http://api.crunchbase.com/v/1/financial-organizations.js", "financial-organizations", "crunchbase" + Path.DirectorySeparatorChar + "financial-organizations.js");


            Console.Write("Parsing financial-organizations names...");
            List<cbFinancialObject> parsedFinancial = financialGenerator.GetFinancials(financialorganizations);
            Console.WriteLine("done");

            Console.Write("Getting each financial-organization info...");

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "financial-organization"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "financial-organization");
            num = 0;
            AllNum = parsedFinancial.Count;
            foreach (cbFinancialObject financial in parsedFinancial)
            {

                DownloadChildFileTask atask = new DownloadChildFileTask() { sFilePathDiff = "financial-organization", slink = financial.permalink, sWebPath = "http://api.crunchbase.com/v/1/financial-organization/" };
                myDownload.cdChildInfoTasks.Add(atask);

                if (num % ThreadNum == 0 || num == AllNum)
                {
                    myDownload.RunChildFileDownloadTask();
                }

              //  myDownload.DownloadChildFile("http://api.crunchbase.com/v/1/financial-organization/", financial.permalink, "financial-organization");

                num++;
                myDownload.ShowDownloadProgress(num, AllNum, "DownLoad financial-organizations:");
            }
            Console.WriteLine("done");

            #endregion

            #region service-providers

            String serviceproviders = myDownload.DownloadParentFileAndGetParentInfo("http://api.crunchbase.com/v/1/service-providers.js", "service-provider", "crunchbase" + Path.DirectorySeparatorChar + "service-providers.js");

            Console.Write("Parsing service-provider names...");
            List<cbServiceproviderObject> parsedServiceProviders = serviceprovidergenerator.GetFinancials(serviceproviders);
            Console.WriteLine("done");

            Console.Write("Getting each service-provider info...");

            if (!Directory.Exists("crunchbase" + Path.DirectorySeparatorChar + "service-provider"))
                Directory.CreateDirectory("crunchbase" + Path.DirectorySeparatorChar + "service-provider");

            num = 0;
            AllNum = parsedServiceProviders.Count;
            foreach (cbServiceproviderObject servicep in parsedServiceProviders)
            {

                DownloadChildFileTask atask = new DownloadChildFileTask() { sFilePathDiff = "service-provider", slink = servicep.permalink, sWebPath = "http://api.crunchbase.com/v/1/service-provider/" };
                myDownload.cdChildInfoTasks.Add(atask);

                if (num % ThreadNum == 0 || num == AllNum)
                {
                    myDownload.RunChildFileDownloadTask();
                }

               // myDownload.DownloadChildFile("http://api.crunchbase.com/v/1/service-provider/", servicep.permalink, "service-provider");
                
                num++;
                myDownload.ShowDownloadProgress(num, AllNum, "DownLoad service-providers:");
            }
            Console.WriteLine("done");

            while (true)
            {
                Console.WriteLine("All download are done,Please Input exit");
                string cmd = Console.ReadLine();
                if (cmd == "exit")
                {
                    break;
                }
            }
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
