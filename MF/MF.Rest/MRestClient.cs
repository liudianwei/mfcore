using Newtonsoft.Json;

using RestSharp;

using System;
using System.Threading.Tasks;

namespace MF.Rest
{
    public class MRestClient
    {
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resource"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        public static string Post(string uri, string resource, object data = null, string token = "", string tokenkey = "Authorization", string contenttype = "application/json; charset=utf-8")
        {
            var client = new RestClient(uri);
            if (tokenkey != "" && token != "")
            {
                client.AddDefaultHeader(tokenkey, token);
            }
            data = data.ToString() == "" ? "{}" : data;
            var request = new RestRequest(resource, Method.POST);
            if (data != null)
            {
                request.AddParameter(contenttype, data, ParameterType.RequestBody);
            }
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resource"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        public async static Task<string> PostAsync(string uri, string resource, object data = null, string token = "", string tokenkey = "Authorization", string contenttype = "application/json; charset=utf-8")
        {
            var client = new RestClient(uri);
            if (tokenkey != "" && token != "")
            {
                client.AddDefaultHeader(tokenkey, token);
            }
            var request = new RestRequest(resource, Method.POST);
            data = data.ToString() == "" ? "{}" : data;
            if (data != null)
            {
                request.AddParameter(contenttype, data, ParameterType.RequestBody);
            }
            IRestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }

        /// <summary>
        ///  Delete
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resource"></param>
        /// <param name="p"></param>
        /// <param name="token"></param>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        public static string Delete(string uri, string resource, object data = null, string token = "", string tokenkey = "Authorization", string contenttype = "application/json; charset=utf-8")
        {
            var client = new RestClient(uri);
            if (tokenkey != "" && token != "")
            {
                client.AddDefaultHeader(tokenkey, token);
            }
            var request = new RestRequest(resource, Method.DELETE);
            data = data.ToString() == "" ? "{}" : data;
            if (data != null)
            {
                request.AddParameter(contenttype, data, ParameterType.RequestBody);
            }
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resource"></param>
        /// <param name="timeout"></param>
        /// <param name="token"></param>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        public static string Get(string uri, string resource, int timeout = 5000, string token = "", string tokenkey = "Authorization", string contenttype = "text/html; charset=utf-8")
        {
            var client = new RestClient(uri);
            if (tokenkey != "" && token != "")
            {
                client.AddDefaultHeader(tokenkey, token);
            }
            var request = new RestRequest(resource, Method.GET)
            {
                Timeout = timeout
            };
            request.AddHeader("content-type", contenttype);
            request.AddHeader("content-encoding", "gzip");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public static void Put(string domain, string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}