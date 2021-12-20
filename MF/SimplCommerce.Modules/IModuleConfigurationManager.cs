using System;
using System.Collections.Generic;
using System.Text;

namespace SimplCommerce.Modules
{
    public interface IModuleConfigurationManager
    {
        IEnumerable<ModuleInfo> GetModules();
    }
}