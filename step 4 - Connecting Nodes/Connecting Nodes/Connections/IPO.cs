using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class IPO: IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Company ADD ATTRIBUTES(DateTime IPOPublishedAt, String IPOStockSymbol, Double IPOValuationAmount, String IPOValuationCurrency)");
            writer.WriteLine("DROP VERTEX TYPE IPO");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company && (model as Company).ipo != null)
                writeIPO(writer, (model as Company).permalink, (model as Company).ipo);
        }

        #region (private, static) writeIPO(System.IO.StreamWriter, String, Model.IPO)

        private static void writeIPO(System.IO.StreamWriter writer, String permalink, Model.IPO iPO)
        {
            writer.Write("UPDATE Company SET(");
            writer.Write(iPO.stock_symbol.GetKeyValueString("IPOStockSymbol")); //StockSymbol seems to be defined for every company with an IPO
            writer.Write(iPO.PublishedAt.GetKeyValueString("IPOPublishedAt").StringWithComma());
            writer.Write(iPO.valuation_amount.GetKeyValueString("IPOValuationAmount").StringWithComma());
            writer.Write(iPO.valuation_currency_code.GetKeyValueString("IPOValuationCurrency").StringWithComma());
            writer.Write(") WHERE ");
            writer.WriteLine(permalink.GetKeyValueString("Permalink"));
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
