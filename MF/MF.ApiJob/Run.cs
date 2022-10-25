using System;
using System.Threading.Tasks;

using MF.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

using Quartz;

namespace ApiJob
{
    [DisallowConcurrentExecution]
    public sealed class Run : IJob
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static DateTime _dtNow = DateTime.Now;
        private static DateTime _expireTime = _dtNow.AddSeconds(-10);
        private static string _tokenT = "";

        public async Task Execute(IJobExecutionContext context)
        {
            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute begin Ver." + Ver.ToString());
            //logger.Info("Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute begin Ver." + Ver.ToString());
            Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Executing ...");
            //logger.Info("Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Executing ...");

            var uri = context.JobDetail.JobDataMap.Get("Target").ToString();
            var resource = context.JobDetail.JobDataMap.Get("TargetDetail").ToString();
            var token = context.JobDetail.JobDataMap.Get("Token").ToString();
            var method = context.JobDetail.JobDataMap.Get("Method").ToString();
            var name = context.JobDetail.JobDataMap.Get("JobName").ToString();
            if (string.IsNullOrWhiteSpace(method))
            {
                logger.Error($"### JobName:{name} 未指定请求方式(get?post?) 请至任务管理界面进行配置 ###");
                return;
            }

            var _token = "";
            var _contenttype = "application/json;charset=UTF-8";
            var _tokenKey = "Authorization";
            var _apiSource = "private";

            if (token != "")
            {
                JObject obj1 = JObject.Parse(token);
                foreach (var item in obj1)
                {
                    switch (item.Key)
                    {
                        case "Content-Type":
                            _contenttype = item.Value.ToString();
                            break;
                        case "tokenKey":
                            _tokenKey = item.Value.ToString();
                            break;
                        case "tokenValue":
                            _token = item.Value.ToString();
                            break;
                        case "apiSource":
                            _apiSource = item.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }
            }
            var jobargs = context.JobDetail.JobDataMap.Get("JobArgs").ToString();
            //string head = $"JobId: {context.JobDetail.Key.Name}" + Environment.NewLine
            //    + $"JobName: {name}" + Environment.NewLine
            //    + $"TotalSeconds: {context.JobRunTime.TotalSeconds}(s)" + Environment.NewLine
            //    + $"FireTime: {TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.DateTime, TimeZoneInfo.Local)}" + Environment.NewLine
            //    + $"NextFireTime: {TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local)}" + Environment.NewLine
            //    + $"Message: " + Environment.NewLine;

            if (_apiSource == "private")
            {
                if (_tokenT!="")
                {
                    _token = _tokenT;
                }
                if (DateTime.Now >= _expireTime)
                {
                    //获取token
                    string resultToken = MRestClient.Get(uri, "rest/usercenter/v1/user/getToken");
                    var results = JsonConvert.DeserializeObject<HttpResults>(resultToken);
                    if (results != null && results.Code == 200 && results.Data != null)
                    {
                        _expireTime = results.Data.ExpireTime.ToLocalTime().AddSeconds(-10);
                        //Console.WriteLine($"过期时间{_expireTime}");
                        _token=_tokenT = $"Bearer {results.Data.Token}";
                    }
                }
            }
            string result = "";
            try
            {
                switch (method)
                {
                    case "get":
                        result = MRestClient.Get(uri, resource + jobargs, 5000, _token, _tokenKey, _contenttype);
                        break;

                    case "post":
                        result = MRestClient.Post(uri, resource, jobargs, _token, _tokenKey, _contenttype);
                        break;

                    case "delete":
                        result = MRestClient.Delete(uri, resource, null, _token, _tokenKey, _contenttype);
                        break;

                    default:
                        break;
                }

                if (!result.Contains("\"status\":\"success\""))
                {
                    logger.Error($"### JobName:{name} {uri}{resource} === {result} ###");
                }
                await Task.Delay(500);
            }
            catch (Exception e)
            {
                logger.Error($"### 异常 {e.Source} === {e.Message} ###");
            }
            finally
            {
                Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute end ");
                //logger.Info("Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute end ");
            }
        }
        public class HttpResults
        {
            public int Code { get; set; }

            public string Status { get; set; }

            public string Message { get; set; }

            public UserLoginResp Data { get; set; }
        }
        public class UserLoginResp
        {
            public string Username { get; set; }
            public string UserId { get; set; }
            public string LoginType { get; set; }
            public DateTime ExpireTime { get; set; }
            public string Token { get; set; }
            public bool Enable { get; set; }
        }
    }
}