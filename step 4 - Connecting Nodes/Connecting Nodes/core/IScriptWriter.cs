using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Crunchbase.Model;
using System.Reflection;

namespace Crunchbase.ConnectingNodes
{
    #region ISciptWriter
    
    /**
     * <summary>IScriptWriter defines the main interface for script writing classes.</summary>
     *
     * Each implementation of IScriptWriter must implement at least one method, to fullfill
     */
    interface IScriptWriter
    {
        /**
         * <summary>Exports schema changing CQL commands.</summary>
         *
         * All implementation should implement their schema changing CQL commands in this method.
         */
        void WriteAlterType(StreamWriter writer);

        /**
         * <summary>Exports CQL commands depending from the given file.</summary>
         *
         * All implementations should implement their file depending CQL commands in this method.
         */
        void Write<MODEL>(StreamWriter writer, MODEL model);

        /**
         * <summary>Returns if the current implementation will write content, if a file with the given type is given to it.  
         * 
         * All implementations should implement this method, depending on their focus. If this method returns false for a given type, the implementation
         * will not be called with objects of this type.
         */
        bool WillWrite(Type type);
        
    }
    #endregion


    #region ScriptWriterFactory
    /**
     * <summary>A factory class for IScriptWriter objects.</summary> 
     */
    static class ScriptWriterFactory
    {
        /**
         * <summary> Returns a IScriptWriter implementation by name.</summary> 
         * <param name="name"> The name of the implementation class with namespace information.</param>
         * <returns> An instance of the given class or null, if the class can not be found or has no default constructor or is not castable to IScriptWriter</returns>
         * 
         * This method constructs an instance of the given class and returns it. The class name must be a fully qualified class name in terms of Assembly.GetType.
         */
        public static IScriptWriter getScriptWriter(String name)
        {
            Type classType = Assembly.GetExecutingAssembly().GetType(name, false, false);
            if (classType == null)
                return null;

            try
            {
                return (IScriptWriter)classType.GetConstructor(Type.EmptyTypes).Invoke(new Object[0]);
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<IScriptWriter>getScriptWriters(params String[] names)
        {
            return names.ToList().ConvertAll((x) => getScriptWriter(x)).FindAll((x) => x != null);
        }
    }
    #endregion
}
