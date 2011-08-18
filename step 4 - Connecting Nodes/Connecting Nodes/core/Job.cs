using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Crunchbase.ConnectingNodes
{
    /**
      * <summary> Job represents one data enrichment for the crunchbase project..</summary>
      */
    class Job : IDisposable
    {
        private Dictionary<Type, List<IScriptWriter>> typeMap = new Dictionary<Type, List<IScriptWriter>>();

        #region (public) properties
        /**
         * <summary> The output stream where the IScriptWriter objects will write their output.</summary>
         * <see cref="ScriptWriter"/>
         */
        public StreamWriter Output { get; set; }

        /**
         * <summary> A list of IScriptWriter objects associated with the job.</summary>
         */
        public List<IScriptWriter> ScriptWriter { get; set; }

        #endregion

        #region (public) methods

        /**
         * <summary> Performs writeAlterType on each IScriptWriter object. </summary>
         * <see cref="IScriptWriter.writeAlterType"/>
         */
        public void writeAlterType()
        {
            if (Output != null)
                ScriptWriter.ForEach((x) => x.WriteAlterType(Output));
        }

        /**
         * <summary> Performs write on each IScriptWriter object. </summary>
         * <see cref="IScriptWriter.write"/>
         */
        public void write<MODEL>(MODEL model)
        {

            if ((model == null) || (Output == null))//nothing to do
                return;

            GetScriptWriterByType(model.GetType()).ForEach((x) => x.Write<MODEL>(Output, model));

        }

        public bool WillWrite(Type type)
        {
            return GetScriptWriterByType(type).Count > 0;
        }

        #endregion

        private List<IScriptWriter> GetScriptWriterByType(Type type)
        {
            //check if the typeMap contains a list of IScriptWriter objects, that will write for the type of file
            if (!typeMap.ContainsKey(type))
                //if not add them
                typeMap.Add(type, ScriptWriter.FindAll((x) => x.WillWrite(type)));
            return typeMap[type];
        }

        #region IDisposable interface

        /**
         * <see cref="IDisposable.Dispose"/>
         */
        public void Dispose()
        {
            if (Output != null)
            {
                Output.Flush();
                Output.Close();
            }
        }
        #endregion
    }

}
