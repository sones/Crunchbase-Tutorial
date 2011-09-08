using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class Relationships : IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Relationship ADD ATTRIBUTES(Company CompanyRelationship, FinancialOrganization FinancialOrganizationRelationship)");
            writer.WriteLine("ALTER VERTEX TYPE Person ADD INCOMINGEDGES (Relationship.Person Relationships)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES (Relationship.CompanyRelationship  Relationships)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES (Relationship.FinancialOrganizationRelationship Relationships)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company)
            {
                if (ErrorLinking.company.Contains((model as Company).permalink))
                    writeRelationship(writer, "CompanyRelationship", (model as Company).permalink, (model as Company).relationships);

            }
            if (model is FinancialOrganization)
            {
                if (ErrorLinking.financialOrganization.Contains((model as FinancialOrganization).permalink))
                    writeRelationship(writer, "FinancialOrganizationRelationship", (model as FinancialOrganization).permalink, (model as FinancialOrganization).relationships);
            }
        }

        #region (private, static) writeRelationship(System.IO.StreamWriter, String, String, List<Relationship>)

        private static void writeRelationship(System.IO.StreamWriter writer, String key, String permalink, List<Relationship> relationships)
        {
            relationships.ForEach((rel) =>
            {
                if (ErrorLinking.person.Contains(rel.person.permalink))
                {
                    writer.Write("INSERT INTO Relationship VALUES (");
                    writer.Write(rel.person.permalink.GetKeyRefString("Person", "Permalink"));
                    writer.Write(rel.title.GetKeyValueString("Title").StringWithComma());
                    writer.Write(rel.is_past.GetKeyValueString("IsPast").StringWithComma());
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
