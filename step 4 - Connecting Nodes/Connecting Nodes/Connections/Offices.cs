using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class Offices : IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Office ADD ATTRIBUTES(Company CompanyOffice, FinancialOrganization FinancialOrganizationOffice, ServiceProvider ServiceProviderOffice)");
            writer.WriteLine("ALTER VERTEX TYPE ServiceProvider ADD INCOMINGEDGES (Office.ServiceProviderOffice Offices)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES (Office.CompanyOffice Offices)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES (Office.FinancialOrganizationOffice Offices)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company)
                if (ErrorLinking.company.Contains((model as Company).permalink))
                    writeOffices(writer, "CompanyOffice", (model as Company).permalink, (model as Company).offices);
            if (model is FinancialOrganization)
                if (ErrorLinking.financialOrganization.Contains((model as FinancialOrganization).permalink))
                    writeOffices(writer, "FinancialOrganizationOffice", (model as FinancialOrganization).permalink, (model as FinancialOrganization).offices);
            if (model is ServiceProvider)
                if (ErrorLinking.serviceProvider.Contains((model as ServiceProvider).permalink))
                    writeOffices(writer, "ServiceProviderOffice", (model as ServiceProvider).permalink, (model as ServiceProvider).offices);
        }

        #region (private, static) writeDegrees(System.IO.StreamWriter, string, string, List<Office>)

        private static void writeOffices(System.IO.StreamWriter writer, string key, string permalink, List<Office> offices)
        {
            offices.ForEach(office =>
            {
                writer.Write("INSERT INTO Office VALUES (");
                writer.Write(permalink.GetKeyRefString(key, "Permalink"));
                writer.Write(office.address1.GetKeyValueString("Address1").StringWithComma());
                writer.Write(office.address2.GetKeyValueString("Address2").StringWithComma());
                writer.Write(office.city.GetKeyValueString("City").StringWithComma());
                writer.Write(office.country_code.GetKeyValueString("CountryCode").StringWithComma());
                writer.Write(office.description.GetKeyValueString("Description").StringWithComma());
                writer.Write(office.latitude.GetKeyValueString("Latitude").StringWithComma());
                writer.Write(office.longitude.GetKeyValueString("Longitude").StringWithComma());
                writer.Write(office.state_code.GetKeyValueString("StateCode").StringWithComma());
                writer.Write(office.zip_code.GetKeyValueString("ZipCode").StringWithComma());
                writer.WriteLine(")");
            });
        }

        #endregion

        #endregion

        #region (public) WillWrite(Type)

        public bool WillWrite(Type type)
        {
            return typeof(Company).IsAssignableFrom(type) || typeof(FinancialOrganization).IsAssignableFrom(type) || typeof(ServiceProvider).IsAssignableFrom(type);
        }

        #endregion

        #endregion
    }
}
