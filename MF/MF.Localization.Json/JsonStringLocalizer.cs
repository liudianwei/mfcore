using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.Extensions.Localization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Folke.Localization.Json
{
    public class JsonStringLocalizer<T> : IStringLocalizer<T>
    {
        private readonly IStringLocalizer localizer;

        public JsonStringLocalizer(IStringLocalizerFactory factory)
        {
            localizer = factory.Create(typeof(T));
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures) => localizer.GetAllStrings(includeAncestorCultures);

        public IStringLocalizer WithCulture(string culture) => ((JsonStringLocalizer)localizer).WithCulture(culture);

        LocalizedString IStringLocalizer.this[string name] => localizer[name];

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => localizer[name, arguments];
    }

    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly string path;
        private readonly string fileName;
        private readonly string defaultFileName;
        private readonly string culture;
        private readonly Dictionary<string, LocalizedString> values = new Dictionary<string, LocalizedString>();

        public JsonStringLocalizer(string resourceFilesDirectory = "lang", string fileName = "MF.Utils.Msg", string defaultFileName = "errMsg", string culture = "zh-CN")
        {
            path = resourceFilesDirectory;
            this.fileName = fileName;
            this.defaultFileName = defaultFileName;
            this.culture = culture;

            var fullPath = $"{resourceFilesDirectory}/{fileName}/{culture ?? "default"}.json";
            if (!File.Exists(fullPath) && culture != null)
                fullPath = $"{resourceFilesDirectory}/{fileName}/default.json";
            if (!File.Exists(fullPath))
                fullPath = $"{resourceFilesDirectory}/{defaultFileName}/{culture ?? "default"}.json";
            if (!File.Exists(fullPath) && culture != null)
                fullPath = $"{resourceFilesDirectory}/{defaultFileName}/default.json";
            var content = File.ReadAllText(fullPath);
            var root = (JObject)JsonConvert.DeserializeObject(content);
            AddKeys(root, null);
        }

        private void AddKeys(JObject fr, string basePath)
        {
            foreach (var property in fr)
            {
                var currentPath = basePath is null ? property.Key : basePath + "." + property.Key;
                var value = property.Value;
                if (value.Type == JTokenType.String)
                {
                    values[currentPath] = new LocalizedString(currentPath, (string)value);
                }
                else
                {
                    AddKeys((JObject)value, currentPath);
                }
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            return values.Values;
        }

        public IStringLocalizer WithCulture(string culture)
        {
            return new JsonStringLocalizer(path, fileName, defaultFileName, culture);
        }

        public LocalizedString Get(string name)
        {
            if (values.TryGetValue(name, out var value))
            {
                return value;
            }
            return new LocalizedString(name, name);
        }

        LocalizedString IStringLocalizer.this[string name]
        {
            get
            {
                return Get(name);

                //if (values.TryGetValue(name, out var value))
                //{
                //    return value;
                //}
                //return new LocalizedString(name, name);
            }
        }

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => new LocalizedString(name, string.Format(values[name].Value, arguments));
    }
}