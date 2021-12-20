using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MF.Core.Runtime
{
    public static class AssemblyUtil
    {
        public static Assembly LoadFromAssemblyName(string assemblyName)
        {
            return Assembly.LoadFrom(assemblyName);
        }
    }
}