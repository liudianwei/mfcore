using RestSharp;
using System;

namespace Mes.Exe.Driver.Rest
{
    /// <summary>
    ///
    /// </summary>
    public static class MesRestClient
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
        public static OperateResultValue Post(string uri, string resource, OperateWriteValue data = null, string mediatype = "application/json", string token = "", string tokenkey = "Authorization")
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
                    request.AddParameter(mediatype, SimpleJson.SerializeObject(data), ParameterType.RequestBody);
                }
                var response = client.Execute(request);
                ResultValue = SimpleJson.DeserializeObject<OperateResultValue>(response.Content);
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
        public static OperateResultValue Delete(string uri, string resource, Parameter p, string mediatype = "application/json", string token = "", string tokenkey = "Authorization")
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
                var response = client.Execute(request);
                ResultValue = SimpleJson.DeserializeObject<OperateResultValue>(response.Content);
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
        public static OperateResultValue Get(string uri, string resource, int timeout = 5000, string mediatype = "text/html; charset=utf-8", string token = "", string tokenkey = "Authorization")
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
                var response = client.Execute(request);
                ResultValue = SimpleJson.DeserializeObject<OperateResultValue>(response.Content);
            }
            catch (Exception e)
            {
                ResultValue.Message = e.Message;
            }
            return ResultValue;
        }
    }

    /// <summary>
    /// 返回值
    /// </summary>
    public class OperateResultValue
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        ///值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 写入值
    /// </summary>
    public class OperateWriteValue
    {
        /// <summary>
        /// TagId
        /// </summary>
        public string TagId { get; set; }

        /// <summary>
        ///  Value
        /// </summary>
        public string Value { get; set; }
    }
}