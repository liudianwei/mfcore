using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SimplCommerce.Modules
{
    public class ModuleConfigurationManager : IModuleConfigurationManager
    {
        private static readonly string ModulesFilename = "modules.json";

        public IEnumerable<ModuleInfo> GetModules()
        {
            var modulesPath = Path.Combine(GlobalConfiguration.ContentRootPath, ModulesFilename);
            using var reader = new StreamReader(modulesPath);
            string content = reader.ReadToEnd();
            var modulesData = JsonSerializer.Deserialize<List<ModuleInfo>>(content);
            return modulesData;
        }
    }
}