using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class Funds: IScriptWriter
    {
        #region IScriptWriter Interface

        #region (public) WriteAlterType(System.IO.StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Fund ADD Attributes(FinancialOrganization Founder)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES(Fund.Founder Funds)");
        }

        #endregion

        #region (public) Write<MODEL>(System.IO.StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is FinancialOrganization)
                writeFinancialOrganization(writer, (model as FinancialOrganization));
        }

        #region (private, static) writeFunds(System.IO.StreamWriter, FinancialOrganization)

        private static void writeFinancialOrganization(System.IO.StreamWriter writer, FinancialOrganization org)
        {
            org.funds.ForEach(fund =>
            {
                writer.Write("INSERT INTO Fund VALUES (");
                writer.Write(org.permalink.GetKeyRefString("Founder", "Permalink"));
                writer.Write(fund.Funded.GetKeyValueString("Funded_At").StringWithComma());
                writer.Write(fund.name.GetKeyValueString("Name").StringWithComma());
                writer.Write(fund.raised_amount.GetKeyValueString("RaisedAmount").StringWithComma());
                writer.Write(fund.raised_currency_code.GetKeyValueString("RaisedCurrencyCode").StringWithComma());
                writer.Write(fund.source_description.GetKeyValueString("SourceDescription").StringWithComma());
                writer.Write(fund.source_url.GetKeyValueString("SourceURL").StringWithComma());
                writer.WriteLine(")");
            });
        }

        #endregion

        #endregion

        #region (public) WillWrite(Type)

        public bool WillWrite(Type type)
        {
            return typeof(FinancialOrganization).IsAssignableFrom(type);
        }

        #endregion

        #endregion
    }
}
