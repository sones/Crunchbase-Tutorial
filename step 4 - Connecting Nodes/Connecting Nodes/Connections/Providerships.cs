using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class Providerships : IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Providership ADD ATTRIBUTES(Company CompanyProvidership, FinancialOrganization FinancialOrganizationProvidership)");
            writer.WriteLine("ALTER VERTEX TYPE ServiceProvider ADD INCOMINGEDGES (Providership.Provider Providerships)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES (Providership.CompanyProvidership  Providerships)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES (Providership.FinancialOrganizationProvidership Providerships)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company)
                if (ErrorLinking.company.Contains((model as Company).permalink))
                    writeProviderships(writer, "CompanyProvidership", (model as Company).permalink, (model as Company).providerships);
            if (model is FinancialOrganization)
                if (ErrorLinking.financialOrganization.Contains((model as FinancialOrganization).permalink))
                    writeProviderships(writer, "FinancialOrganizationProvidership", (model as FinancialOrganization).permalink, (model as FinancialOrganization).providerships);
        }

        #region (private, static) writeProviderships(System.IO.StreamWriter, String, String, List<Providership>)

        private static void writeProviderships(System.IO.StreamWriter writer, String key, String permalink, List<Providership> providerships)
        {
            providerships.ForEach((pro) =>
            {
                if (ErrorLinking.serviceProvider.Contains(pro.provider.permalink))
                {
                    writer.Write("INSERT INTO Providership VALUES (");
                    writer.Write(pro.provider.permalink.GetKeyRefString("Provider", "Permalink"));
                    writer.Write(pro.title.GetKeyValueString("Title").StringWithComma());
                    writer.Write(pro.is_past.GetKeyValueString("IsPast").StringWithComma());
                    writer.Write(permalink.GetKeyRefString(key, "Permalink").StringWithComma());
                    writer.WriteLine(")");
                }
            });
        }

        #endregion

        #endregion

        #region (public) WillWrite(Type)

        public bool WillWrite(Type type)
        {
            return typeof(Company).IsAssignableFrom(type) || typeof(FinancialOrganization).IsAssignableFrom(type);
        }

        #endregion

        #endregion
    }
}
