using Jil;
using System;
using System.Collections.Generic;
using System.IO;

namespace MF.Utils.Json
{
    /// <summary>
    /// https://github.com/kevin-montrose/Jil Jil>Newtonsoft.Json
    /// </summary>
    public static class JilH
    {
        /// <summary>
        ///jilH在序列化、反序列化json比 Newtonsoftjson快
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JilToJson(this object obj)
        {
            return JSON.Serialize(obj);
        }

        public static string JilToJson(this object obj, Options options)
        {
            return JSON.Serialize(obj, options);
        }

        public static string JilToJsonCamelCase(this object obj)
        {
            return JSON.Serialize(obj, new Options(includeInherited: true, dateFormat: DateTimeFormat.ISO8601, serializationNameFormat: SerializationNameFormat.CamelCase));
        }

        public static void JilToJson(this object obj, TextWriter output, Options options = null)
        {
            JSON.Serialize(obj, output, options);
        }

        public static T JilToObject<T>(this string json)
        {
            using var input = new StringReader(json);
            var result = JSON.Deserialize<T>(input);
            return result;
        }

        public static T JilToObjectCamelCase<T>(this string json)
        {
            using var input = new StringReader(json);
            var options = new Options(includeInherited: true,
                dateFormat: DateTimeFormat.ISO8601, serializationNameFormat: SerializationNameFormat.CamelCase);
            var result = JSON.Deserialize<T>(input, options);
            return result;
        }

        public static object JilToObjectCamelCase(this string json, Type type)
        {
            using var input = new StringReader(json);
            var options = new Options(includeInherited: true,
                dateFormat: DateTimeFormat.ISO8601, serializationNameFormat: SerializationNameFormat.CamelCase);
            var result = JSON.Deserialize(input, type, options);
            return result;
        }

        public static T JilToObject<T>(this StreamReader streamReader)
        {
            return JSON.Deserialize<T>(streamReader);
        }

        public static List<T> JilToList<T>(this string json)
        {
            using var input = new StringReader(json);
            var result = JSON.Deserialize<List<T>>(input);
            return result;
        }
    }
}