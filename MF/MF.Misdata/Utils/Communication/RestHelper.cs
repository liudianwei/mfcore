using Newtonsoft.Json;

using RestSharp;

using System;

namespace Common.Communication
{
    /// <summary>
    ///
    /// </summary>
    public static class RestHelper
    {
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resource"></param>
        /// <param name="data"></param>
        /// <param name="mediatype"></param>
        /// <param name="token"></param>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        public static OperateResultValue Post(string uri,
                                              string resource,
                                              OperateWriteValue data = null,
                                              string mediatype = "application/json",
                                              string token = "",
                                              string tokenkey = "Authorization")
        {
            OperateResultValue ResultValue = new OperateResultValue() { ErrorCode = -1, IsSuccess = false, Value = "" };
            try
            {
                var client = new RestClient(uri);
                if (tokenkey != "" && token != "")
                {
                    client.AddDefaultHeader(tokenkey, token);
                }
                var request = new RestRequest(resource, Method.POST);
                if (data != null)
                {
                    request.AddParameter(mediatype, JsonConvert.SerializeObject(data), ParameterType.RequestBody);
                }
                IRestResponse response = client.Execute(request);
                ResultValue = JsonConvert.DeserializeObject<OperateResultValue>(response.Content);
            }
            catch (Exception e)
            {
                ResultValue.Message = e.Message;
            }
            return ResultValue;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resource"></param>
        /// <param name="p"></param>
        /// <param name="mediatype"></param>
        /// <param name="token"></param>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        [Obsolete]
        public static OperateResultValue Delete(string uri,
                                                string resource,
                                                Parameter p,
                                                string mediatype = "application/json",
                                                string token = "",
                                                string tokenkey = "Authorization")
        {
            OperateResultValue ResultValue = new OperateResultValue() { ErrorCode = -1, IsSuccess = false, Value = "" };
            try
            {
                var client = new RestClient(uri);
                if (tokenkey != "" && token != "")
                {
                    client.AddDefaultHeader(tokenkey, token);
                }
                var request = new RestRequest(resource, Method.DELETE);
                request.AddParameter(p);
                request.AddHeader("content-type", mediatype);
                IRestResponse response = client.Execute(request);
                ResultValue = JsonConvert.DeserializeObject<OperateResultValue>(response.Content);
            }
            catch (Exception e)
            {
                ResultValue.Message = e.Message;
            }
            return ResultValue;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resource"></param>
        /// <param name="timeout"></param>
        /// <param name="mediatype"></param>
        /// <param name="token"></param>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        public static OperateResultValue Get(string uri,
                                             string resource,
                                             int timeout = 5000,
                                             string mediatype = "text/html; charset=utf-8",
                                             string token = "",
                                             string tokenkey = "Authorization")
        {
            OperateResultValue ResultValue = new OperateResultValue() { ErrorCode = -1, IsSuccess = false, Value = "" };
            try
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
                request.AddHeader("content-type", mediatype);
                request.AddHeader("content-encoding", "gzip");
                IRestResponse response = client.Execute(request);
                ResultValue = JsonConvert.DeserializeObject<OperateResultValue>(response.Content);
            }
            catch (Exception e)
            {
                ResultValue.Message = e.Message;
            }
            return ResultValue;
        }
    }
}