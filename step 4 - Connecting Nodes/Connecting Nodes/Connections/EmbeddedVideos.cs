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
     * embedded videos and loads the data. 
     * Embedded videos can be assigned to persons, products, companies and financial organizations.
     * 
     */

    public class EmbeddedVideos: IScriptWriter
    {
        #region IScriptWriter interface

        #region (public) writeAlterType(StreamWriter)

        public void WriteAlterType(System.IO.StreamWriter writer)
        {
            writer.WriteLine("ALTER VERTEX TYPE EmbeddedVideo ADD ATTRIBUTES(Company CompanyVideo, FinancialOrganization FinancialOrganizationVideo, Product ProductVideo, Person PersonVideo)");
            writer.WriteLine("ALTER VERTEX TYPE Company ADD INCOMINGEDGES(EmbeddedVideo.CompanyVideo EmbeddedVideos)");
            writer.WriteLine("ALTER VERTEX TYPE FinancialOrganization ADD INCOMINGEDGES(EmbeddedVideo.FinancialOrganizationVideo EmbeddedVideos)");
            writer.WriteLine("ALTER VERTEX TYPE Product ADD INCOMINGEDGES(EmbeddedVideo.ProductVideo EmbeddedVideos)");
            writer.WriteLine("ALTER VERTEX TYPE Person ADD INCOMINGEDGES(EmbeddedVideo.PersonVideo EmbeddedVideos)");
        }

        #endregion

        #region (public) write<MODEL>(StreamWriter, MODEL)

        public void Write<MODEL>(System.IO.StreamWriter writer, MODEL model)
        {
            if (model == null)
                return;
            if (model is Company)
                writeEmbeddedVideos(writer, "CompanyVideo", (model as Company).permalink, (model as Company).video_embeds);
            if (model is FinancialOrganization)
                writeEmbeddedVideos(writer, "FinancialOrganizationVideo", (model as FinancialOrganization).permalink, (model as FinancialOrganization).video_embeds);
            if (model is Person)
                writeEmbeddedVideos(writer, "PersonVideo", (model as Person).permalink, (model as Person).video_embeds);
            if (model is Product)
                writeEmbeddedVideos(writer, "ProductVideo", (model as Product).permalink, (model as Product).video_embeds);
        }

        #region (private, static) writeEmbeddedVideos(System.IO.StreamWriter, string, string, List<Link>)

        private static void writeEmbeddedVideos(System.IO.StreamWriter writer, string key, string permalink, List<Video> videos)
        {
            videos.ForEach(video =>
            {
                writer.Write("INSERT INTO EmbeddedVideo VALUES (");
                writer.Write(permalink.GetKeyRefString(key, "Permalink"));
                writer.Write(video.embed_code.GetKeyValueString("EmbedCode").StringWithComma());
                writer.Write(video.description.GetKeyValueString("Description").StringWithComma());
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
