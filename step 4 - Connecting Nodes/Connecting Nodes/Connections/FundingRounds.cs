using System;
using System.Linq;
using Crunchbase.Model;
using System.Collections.Generic;
using Crunchbase.ConnectingNodes;

namespace Crunchbase.ConnectingNodes.Connections
{
    /**
     * 
     * This class implements the IScriptWriter interface, to create a GQL script, that enriches the crunchbase schema with
     * investments and loads the data. 
     * An investment can be raised by companies, financial organisations or persons.
     * 
     */
    public class FundingRounds : IScriptWriter
    {
        #region IScriptWriter Interface

        #region (public) WriteAlterType(System.IO.StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE FundingRound ADD ATTRIBUTES(SET<Company> CompanyInvestors, SET<FinancialOrganization> FinancialOrganizationInvestors, SET<Person> PersonInvestors)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES(FundingRound.FinancialOrganizationInvestors Investments)");
            writer.WriteLine("ALTER VERTEX TYPE Person ADD INCOMINGEDGES(FundingRound.PersonInvestors Investments)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES(FundingRound.CompanyInvestors Investments)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES(FundingRound.Company FundingRounds)");
        }

        #endregion

        #region (public) Write<MODEL>(System.IO.StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company)
                if (ErrorLinking.company.Contains((model as Company).permalink))
                    writeCompany(writer, (model as Company));
        }

        #region (private, static) writeCompany(System.IO.StreamWriter, Company)

        private static void writeCompany(System.IO.StreamWriter writer, Company company)
        {
            company.funding_rounds.ForEach(fundingRound =>
            {
                writer.Write("INSERT INTO FundingRound VALUES (");
                writer.Write(company.permalink.GetKeyRefString("Company", "Permalink"));
                writer.Write(fundingRound.FundedAt.GetKeyValueString("Funded_At").StringWithComma());
                writeInvestors(writer, fundingRound.investments);
                writer.Write(fundingRound.raised_amount.GetKeyValueString("RaisedAmount").StringWithComma());
                writer.Write(fundingRound.raised_currency_code.GetKeyValueString("RaisedCurrencyCode").StringWithComma());
                writer.Write(fundingRound.round_code.GetKeyValueString("RoundCode").StringWithComma());
                writer.Write(fundingRound.source_description.GetKeyValueString("SourceDescription").StringWithComma());
                writer.Write(fundingRound.source_url.GetKeyValueString("SourceURL").StringWithComma());
                writer.WriteLine(")");
            });
        }

        #region (private, static) writeInvestors(System.IO.StreamWriter, List<Dictionary<String, Permalink>>)

        private static void writeInvestors(System.IO.StreamWriter writer, List<Dictionary<String, Permalink>> investors)
        {
            if (investors != null && investors.Count > 0)
            {

                writer.Write(GetPermalinks(investors, "company").GetSETOF("CompanyInvestors", "Permalink").StringWithComma());
                writer.Write(GetPermalinks(investors, "financial_org").GetSETOF("FinancialOrganizationInvestors", "Permalink").StringWithComma());
                writer.Write(GetPermalinks(investors, "person").GetSETOF("PersonInvestors", "Permalink").StringWithComma());
            }
        }

        #region (private, static) GetSETOF(System.IO.StreamWriter, String, List<string>)

        #endregion

        #endregion

        #region (private, static) GetPermalinks<K>(List<Dictionary<K, Permalink>>, K)

        private static List<String> GetPermalinks<K>(List<Dictionary<K, Permalink>> s, K key)
        {
            return s.FindAll(x => x[key] != null).ConvertAll(x => x[key].permalink);
        }

        #endregion

        #endregion

        #endregion

        #region (public) WillWrite(Type)

        public bool WillWrite(Type type)
        {
            return typeof(Company).IsAssignableFrom(type);
        }

        #endregion

        #endregion
    }
}
