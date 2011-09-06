using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Crunchbase.Model;
using System.Globalization;
using System.Threading;

namespace Crunchbase.ConnectingNodes
{
    static class ErrorLinking
    {



        public static List<string> company { get; set; }
        public static List<string> person { get; set; }
        public static List<string> product { get; set; }
        public static List<string> financialOrganization { get; set; }
        public static List<string> serviceProvider { get; set; }
    }
    /**
     * <summary>Crunchbase.ConnectingNodes is a small tool to create gql scripts to enrich existing crunchbase gql script data.</summary>
     */
    class ConnectingNodes
    {
        #region (private) fields
        private List<string> companyFiles;
        private List<string> personFiles;
        private List<string> financeFiles;
        private List<string> productFiles;
        private List<string> serviceFiles;
        private List<Job> jobs;
        #endregion

        #region c'tors

        public ConnectingNodes(string[] companyFiles, string[] personFiles, string[] financialOrgFiles, string[] productFiles, string[] serviceProviderFiles, params Job[] jobs)
        {
            this.companyFiles = new List<String>(companyFiles.Distinct());
            this.personFiles = new List<string>(personFiles.Distinct());
            this.financeFiles = new List<String>(financialOrgFiles.Distinct());
            this.productFiles = new List<string>(productFiles.Distinct());
            this.serviceFiles = new List<String>(serviceProviderFiles.Distinct());
            this.jobs = new List<Job>(jobs);
        }

        #endregion

        #region (public) methods
        public void Run()
        {
            jobs.ForEach((j) => j.writeAlterType());

            RunJobs<Company>(companyFiles);
            RunJobs<Person>(personFiles);
            RunJobs<FinancialOrganization>(financeFiles);
            RunJobs<Product>(productFiles);
            RunJobs<ServiceProvider>(serviceFiles);

            jobs.ForEach((j) => j.Dispose());
        }
        #endregion

        #region Main
        static void Main(string[] args)
        {
            #region user information
            Console.WriteLine("Crunchbase Connecting Nodes Import tool");
            Console.WriteLine();
            Console.WriteLine("A small tool to create gql scripts to enrich existing crunchbase gql script");
            Console.WriteLine("data.");
            Console.WriteLine();

            #endregion

            #region get JSON files

            string indir = (args.Length == 0) ? "." : args[0];
            string outdir = (args.Length <= 1) ? "." : args[1];
            string[] CompanyFiles = Directory.GetFiles(indir + Path.DirectorySeparatorChar + "company" + Path.DirectorySeparatorChar, "*.js");
            string[] PersonFiles = Directory.GetFiles(indir + Path.DirectorySeparatorChar + "person" + Path.DirectorySeparatorChar, "*.js");
            string[] FinancialOrgFiles = Directory.GetFiles(indir + Path.DirectorySeparatorChar + "financial-organization" + Path.DirectorySeparatorChar, "*.js");
            string[] ProductFiles = Directory.GetFiles(indir + Path.DirectorySeparatorChar + "product" + Path.DirectorySeparatorChar, "*.js");
            string[] ServiceProviderFiles = Directory.GetFiles(indir + Path.DirectorySeparatorChar + "service-provider" + Path.DirectorySeparatorChar, "*.js");

            #endregion

            #region do main job

            //Here you can define jobs.
            //Each job has an output(file|stream) and connected SciptWriter objects. 
            //The ScriptWriter objects do the entire work of writing text to the output.
            //Make sure to never define two jobs writing to the same output.
            ConnectingNodes prog = new ConnectingNodes(CompanyFiles, PersonFiles, FinancialOrgFiles, ProductFiles, ServiceProviderFiles,
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_4_Relationships.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.Relationships") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_5_FoundingRounds.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.FundingRounds") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_6_Funds.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.Funds") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_7_Milestones.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.Milestones") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_8_Acquisitions.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.Acquisitions") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_9_Providerships.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.Providerships") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_10_Offices.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.Offices") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_11_Degrees.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.Degrees") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_12_IPO.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.IPO") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_13_ExternalLinks.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.ExternalLinks") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_14_EmbeddedVideos.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.EmbeddedVideos") },
                new Job() { Output = File.CreateText(outdir + Path.DirectorySeparatorChar + "Step_15_WebPresences.gql"), ScriptWriter = ScriptWriterFactory.getScriptWriters("Crunchbase.ConnectingNodes.Connections.WebPresences") });



            ErrorLinking.company = new List<string>();
            ErrorLinking.person = new List<string>();
            ErrorLinking.product = new List<string>();
            ErrorLinking.financialOrganization = new List<string>();
            ErrorLinking.serviceProvider = new List<string>();


            ErrorLinking.company = File.ReadAllLines("Company.txt").ToList();
            ErrorLinking.person = File.ReadAllLines("Person.txt").ToList();
            ErrorLinking.product = File.ReadAllLines("Product.txt").ToList();
            ErrorLinking.financialOrganization = File.ReadAllLines("FinancialOrganisation.txt").ToList();
            ErrorLinking.serviceProvider = File.ReadAllLines("ServiceProvider.txt").ToList();


            prog.Run();
            #endregion
        }
        #endregion

        #region (private) RunJobs(List<string>)

        /**
         * <summary> Finds all jobs, that contains IScriptWriter, that are ready to handle the MODEL and executes this jobs. </summary>
         */
        private void RunJobs<MODEL>(List<string> files) where MODEL : class
        {
            var subset = jobs.FindAll((job) => job.WillWrite(typeof(MODEL)));

            if (subset.Count > 0)
            {
                Console.WriteLine(string.Format("Perform jobs for type {0}", typeof(MODEL).Name));

                files.ForEach((file) =>
                {
                    MODEL model = File.ReadAllText(file).DeserializeJSON<MODEL>();
                    subset.ForEach((job) => job.write(model));
                });
            }

        }

        #endregion

    }
}
