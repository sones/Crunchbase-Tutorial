using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class ExternalLinks : IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE ExternalLink ADD ATTRIBUTES(Company CompanyLink, FinancialOrganization FinancialOrganizationLink, Product ProductLink, Person PersonLink, ServiceProvider ServiceProviderLink)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES(ExternalLink.CompanyLink ExternalLinks)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES(ExternalLink.FinancialOrganizationLink ExternalLinks)");
            writer.WriteLine("ALTER VERTEX TYPE Product ADD INCOMINGEDGES(ExternalLink.ProductLink ExternalLinks)");
            writer.WriteLine("ALTER VERTEX TYPE Person ADD INCOMINGEDGES(ExternalLink.PersonLink ExternalLinks)");
            writer.WriteLine("ALTER VERTEX TYPE ServiceProvider ADD INCOMINGEDGES(ExternalLink.ServiceProviderLink ExternalLinks)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company)
                if (ErrorLinking.company.Contains((model as Company).permalink))
                    writeExternalLinks(writer, "CompanyLink", (model as Company).permalink, (model as Company).external_links);
            if (model is FinancialOrganization)
                if (ErrorLinking.financialOrganization.Contains((model as FinancialOrganization).permalink))
                    writeExternalLinks(writer, "FinancialOrganizationLink", (model as FinancialOrganization).permalink, (model as FinancialOrganization).external_links);
            if (model is Person)
                if (ErrorLinking.person.Contains((model as Person).permalink))
                    writeExternalLinks(writer, "PersonLink", (model as Person).permalink, (model as Person).external_links);
            if (model is Product)
                if (ErrorLinking.product.Contains((model as Product).permalink))
                    writeExternalLinks(writer, "ProductLink", (model as Product).permalink, (model as Product).external_links);
            if (model is ServiceProvider)
                if (ErrorLinking.serviceProvider.Contains((model as ServiceProvider).permalink))
                    writeExternalLinks(writer, "ServiceProviderLink", (model as ServiceProvider).permalink, (model as ServiceProvider).external_links);

        }

        #region (private, static) writeExternalLinks(System.IO.StreamWriter, string, string, List<Link>)

        private static void writeExternalLinks(System.IO.StreamWriter writer, string key, string permalink, List<Link> links)
        {
            links.ForEach(link =>
            {
                writer.Write("INSERT INTO ExternalLink VALUES (");
                writer.Write(permalink.GetKeyRefString(key, "Permalink"));
                writer.Write(link.external_url.GetKeyValueString("ExternalURL").StringWithComma());
                writer.Write(link.title.GetKeyValueString("Title").StringWithComma());
                writer.WriteLine(")");
            });
        }

        #endregion

        #endregion

        #region (public) WillWrite(Type)

        public bool WillWrite(Type type)
        {
            return typeof(Company).IsAssignableFrom(type) || typeof(FinancialOrganization).IsAssignableFrom(type) || typeof(Product).IsAssignableFrom(type) || typeof(Person).IsAssignableFrom(type) || typeof(ServiceProvider).IsAssignableFrom(type);
        }

        #endregion

        #endregion
    }
}
