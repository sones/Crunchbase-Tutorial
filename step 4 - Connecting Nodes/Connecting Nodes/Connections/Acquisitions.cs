using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    /**
     * 
     * This class implements the IScriptWriter interface, to create a GQL script, that enriches the crunchbase schema with
     * acquisition and loads the data. 
     * Only companies can aquire companies.
     * 
     */
    public class Acquisitions : IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Acquisition RENAME ATTRIBUTE Company TO Acquirer");
            writer.WriteLine("ALTER VERTEX TYPE Acquisition ADD ATTRIBUTES(Company Acquiree)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES(Acquisition.Acquirer Acquisitions, Acquisition.Acquiree Acquisition)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company)
                if (ErrorLinking.company.Contains((model as Company).permalink))
                    writeAcquisition(writer, (model as Company).permalink, (model as Company).acquisitions);

        }

        #region (private, static) writeAcquisition(System.IO.StreamWriter, string, List<Model.Acquisition>)

        private static void writeAcquisition(System.IO.StreamWriter writer, string permalink, List<Model.Acquisition> acquisitions)
        {
            acquisitions.ForEach(acquisition =>
            {
                if (ErrorLinking.company.Contains(acquisition.company.permalink))
                {
                    writer.Write("INSERT INTO Acquisition VALUES (");
                    writer.Write(permalink.GetKeyRefString("Acquirer", "Permalink"));
                    writer.Write(acquisition.company.permalink.GetKeyRefString("Acquiree", "Permalink").StringWithComma());
                    writer.Write(acquisition.AcquiredAt.GetKeyValueString("Acquired_At").StringWithComma());
                    writer.Write(acquisition.price_amount.GetKeyValueString("Price").StringWithComma());
                    writer.Write(acquisition.price_currency_code.GetKeyValueString("PriceCurrency").StringWithComma());
                    writer.Write(acquisition.source_description.GetKeyValueString("SourceDestination").StringWithComma());
                    writer.Write(acquisition.source_url.GetKeyValueString("SourceURL").StringWithComma());
                    writer.Write(acquisition.term_code.GetKeyValueString("TermCode").StringWithComma());
                    writer.WriteLine(")");
                }
            });
        }

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
