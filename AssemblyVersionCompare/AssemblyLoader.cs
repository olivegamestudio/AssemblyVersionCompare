using System;
using System.Reflection;

namespace AssemblyVersionCompare
{
    class AssemblyLoader : MarshalByRefObject
    {
        public Version Load(string file)
        {
            var assembly = Assembly.LoadFile(file);
            return assembly.GetName().Version;
        }
    }
}