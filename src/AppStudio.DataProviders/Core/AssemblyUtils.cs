using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.DataProviders
{
    internal class AssemblyUtils
    {
        internal static string GetVersion()
        {
            var assembly = typeof(AssemblyUtils).GetTypeInfo().Assembly;
            var assemblyName = new AssemblyName(assembly.FullName);
            return $"{assemblyName.Version.Major}.{assemblyName.Version.Minor}.{assemblyName.Version.Build}";
        }
    }
}
