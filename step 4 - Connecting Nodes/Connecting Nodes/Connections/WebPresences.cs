using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crunchbase.Model;

namespace Crunchbase.ConnectingNodes.Connections
{
    public class WebPresences: IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("CREATE VERTEX TYPE WebPresence ATTRIBUTES(Person Person, String ExternalURL, String Title )");
            writer.WriteLine("ALTER VERTEX TYPE Person ADD INCOMINGEDGES(WebPresence.Person WebPresences)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Person)
                writeWebPresences(writer, (model as Person).permalink, (model as Person).web_presences);

        }

        #region (private, static) writeWebPresences(System.IO.StreamWriter, string, List<WebPresence>)

        private static void writeWebPresences(System.IO.StreamWriter writer, string permalink, List<WebPresence> presences)
        {
            presences.ForEach(presence =>
            {
                writer.Write("INSERT INTO WebPresence VALUES (");
                writer.Write(permalink.GetKeyRefString("Person", "Permalink"));
                writer.Write(presence.external_url.GetKeyValueString("ExternalURL").StringWithComma());
                writer.Write(presence.title.GetKeyValueString("Title").StringWithComma());
                writer.WriteLine(")");
            });
        }

        #endregion

        #endregion

        #region (public) WillWrite(Type)

        public bool WillWrite(Type type)
        {
            return typeof(Person).IsAssignableFrom(type);
        }

        #endregion

        #endregion

    }
}
