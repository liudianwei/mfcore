using System;
using System.Text.Json;

namespace MF.Utils.Json
{
    /// <summary>
    /// </summary>
    public static class TextJson
    {
        private readonly static Lazy<JsonSerializerOptions> _instance = new Lazy<JsonSerializerOptions>(() => {
            var options = new JsonSerializerOptions
            {
                Converters = { new DynamicJsonConverter() },
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                // ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };
            return options;
        });

        public static JsonSerializerOptions Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, Instance);
        }


        public static T ToObj<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, Instance);
        }


    }
}