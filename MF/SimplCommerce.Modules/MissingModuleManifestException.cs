using System;
using System.Collections.Generic;
using System.Text;

namespace SimplCommerce.Modules
{
    public class MissingModuleManifestException : Exception
    {
        public string ModuleName { get; }

        public MissingModuleManifestException()
        {
        }

        public MissingModuleManifestException(string message)
            : base(message)
        {
        }

        public MissingModuleManifestException(string message, string moduleName)
            : this(message)
        {
            ModuleName = moduleName;
        }

        public MissingModuleManifestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}