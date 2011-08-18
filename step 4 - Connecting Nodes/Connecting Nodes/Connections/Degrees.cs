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
     * Degrees and loads the data. 
     * Only persons can have degrees.
     * 
     */

    public class Degrees: IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE Degree ADD ATTRIBUTES(Person Person)");
            writer.WriteLine("ALTER VERTEX TYPE Person ADD INCOMINGEDGES (Degree.Person Degrees)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Person)
                writeDegrees(writer, (model as Person).permalink, (model as Person).degrees);
        }

        #region (private, static) writeDegrees(System.IO.StreamWriter, string, List<Degree>)

        private static void writeDegrees(System.IO.StreamWriter writer, string permalink, List<Degree> degrees)
        {
            degrees.ForEach(degree =>
            {
                writer.Write("INSERT INTO Degree VALUES (");
                writer.Write(permalink.GetKeyRefString("Person", "Permalink"));
                writer.Write(degree.degree_type.GetKeyValueString("DegreeType").StringWithComma());
                writer.Write(degree.GraduatedAt.GetKeyValueString("Graduated_At").StringWithComma());
                writer.Write(degree.institution.GetKeyValueString("Institution").StringWithComma());
                writer.Write(degree.subject.GetKeyValueString("Subject").StringWithComma());
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
