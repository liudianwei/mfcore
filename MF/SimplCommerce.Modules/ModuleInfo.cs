using System.Reflection;
using System.Text.Json.Serialization;

namespace SimplCommerce.Modules
{
    public class ModuleInfo
    {
        /// <summary>
        /// id 唯一，程序集的名称
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isBundledWithHost")]
        public bool IsBundledWithHost { get; set; }

        [JsonPropertyName("lastVersion")]
        public string LastVersion { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonIgnore]
        public Assembly Assembly { get; set; }
    }
}