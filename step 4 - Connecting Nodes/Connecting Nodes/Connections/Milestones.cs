using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;
using Crunchbase.ConnectingNodes;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class Milestones: IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Milestone ADD ATTRIBUTES(Person PersonMilestone, Product ProductMilestone, FinancialOrganization FinancialOrganizationMilestone, Company CompanyMilestone)");
            writer.WriteLine("ALTER VERTEX TYPE Person ADD INCOMINGEDGES (Milestone.PersonMilestone Milestones)");
            writer.WriteLine("ALTER VERTEX TYPE Product ADD INCOMINGEDGES (Milestone.ProductMilestone Milestones)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES (Milestone.FinancialOrganizationMilestone Milestones)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES (Milestone.CompanyMilestone Milestones)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Person)
                writeMilestone(writer, "PersonMilestone", (model as Person).permalink, (model as Person).milestones);
            if (model is Product)
                writeMilestone(writer, "ProductMilestone", (model as Product).permalink, (model as Product).milestones);
            if (model is FinancialOrganization)
                writeMilestone(writer, "FinancialOrganizationMilestone", (model as FinancialOrganization).permalink, (model as FinancialOrganization).milestones);
            if (model is Company)
                writeMilestone(writer, "CompanyMilestone", (model as Company).permalink, (model as Company).milestones);

        }

        #region (private, static) writeMilestone(System.IO.StreamWriter, string, string, List<Milestone>)

        private static void writeMilestone(System.IO.StreamWriter writer, string key, string permalink, List<Milestone> milestones)
        {
            milestones.ForEach(stone =>
                {
                    writer.Write("INSERT INTO Milestone VALUES (");
                    writer.Write(permalink.GetKeyRefString(key, "Permalink"));
                    writer.Write(stone.description.GetKeyValueString("Description").StringWithComma());
                    writer.Write(stone.source_description.GetKeyValueString("SourceDescription").StringWithComma());
                    writer.Write(stone.source_url.GetKeyValueString("SourceURL").StringWithComma());
                    writer.Write(stone.StonedAt.GetKeyValueString("Stoned_At").StringWithComma());
                    writer.WriteLine(")");
                });
        }

        #endregion

        #endregion

        #region (public) WillWrite(Type)

        public bool WillWrite(Type type)
        {
            return typeof(Company).IsAssignableFrom(type) || typeof(FinancialOrganization).IsAssignableFrom(type) || typeof(Product).IsAssignableFrom(type) || typeof(Person).IsAssignableFrom(type);
        }

        #endregion

        #endregion
    }
}
