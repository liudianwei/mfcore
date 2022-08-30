using MF.Rest;

using Newtonsoft.Json;

using NLog;

using Quartz;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MF.Job.JobItems
{
    [DisallowConcurrentExecution]
    public sealed class ExportExcelJob : IJob
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static DateTime _dtNow = DateTime.Now;
        private static DateTime _expireTime = _dtNow.AddSeconds(-10);
        private static string _token = "";

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
            //var method = context.JobDetail.JobDataMap.Get("Method").ToString();
            //var jobargs = context.JobDetail.JobDataMap.Get("JobArgs").ToString();
            var name = context.JobDetail.JobDataMap.Get("JobName").ToString();
            //string head = $"JobId: {context.JobDetail.Key.Name}" + Environment.NewLine
            //    + $"JobName: {name}" + Environment.NewLine
            //    + $"TotalSeconds: {context.JobRunTime.TotalSeconds}(s)" + Environment.NewLine
            //    + $"FireTime: {TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.DateTime, TimeZoneInfo.Local)}" + Environment.NewLine
            //    + $"NextFireTime: {TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local)}" + Environment.NewLine
            //    + $"Message: " + Environment.NewLine;

            try
            {
                //查询未完成的导出任务
                string resultstr = MRestClient.Get(uri, resource);

                var result = JsonConvert.DeserializeObject<RestResult>(resultstr);

                if (result != null && result.Data != null && result.Data.Count > 0)
                {
                    var status1list = result.Data.Where(i => i.Status == "1").ToList();

                    //判断是否有正在执行中的
                    //没有就取第一个去执行
                    if (status1list.Count == 0)
                    {
                        var list = result.Data;
                        if (_dtNow >= _expireTime)
                        {
                            //获取token
                            string resultToken = MRestClient.Get(uri, "rest/usercenter/v1/user/getToken");
                            var results = JsonConvert.DeserializeObject<HttpResults>(resultToken);
                            if (results != null && results.Code == 200 && results.Data != null)
                            {
                                _expireTime = results.Data.ExpireTime.ToLocalTime();
                                _token = results.Data.Token;
                            }
                        }
                        Console.WriteLine($"本次执行一共检测到 {list.Count}条 待执行任务");
                        //logger.Info($"本次执行一共检测到 {list.Count}条 待执行任务");
                        list.ForEach(task => {
                            var obj = JsonConvert.DeserializeObject<RestQuery>(task.QueryItem);
                            var args = new { moduleName = task.ModuleName, taskId = task.Id, Condition = obj.Condition, Condition2 = obj.Condition2 };
                            //执行任务
                            resultstr = MRestClient.Post(task.Url, "", JsonConvert.SerializeObject(args), $"Bearer {_token}");

                            if (!resultstr.Contains("\"status\":\"success\"")){
                                logger.Error($"### 任务执行结果:{resultstr}### ");
                            }
                        });
                    }
                }

                try
                {
                    if (result?.Code != "200")
                    {
                        logger.Error($"### 导出任务查询失败 {uri}/{resource} === {result} ###");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error($"### 异常 {ex.Source} === {ex.Message} ###");
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

        public class ExportLogDto
        {
            public string Id { get; set; }

            public string ModuleName { get; set; }

            public string FileName { get; set; }

            public string FileSize { get; set; }

            public string Progress { get; set; }

            public string QueryItem { get; set; }

            public System.DateTime? StartTime { get; set; }

            public System.DateTime? EndTime { get; set; }

            public string DownloadPath { get; set; }

            public string Duration { get; set; }

            public string Creator { get; set; }

            public System.DateTime CreateTime { get; set; }

            public string Updator { get; set; }

            public System.DateTime UpdateTime { get; set; }

            public string Remark { get; set; }

            public string State { get; set; }

            public int InnerVersion { get; set; }

            public string Status { get; set; }

            public int DownloadTimes { get; set; }

            public string Url { get; set; }

            public string Token { get; set; }
        }

        public class RestResult
        {
            public string Code { get; set; }
            public List<ExportLogDto> Data { get; set; }
            public string Message { get; set; }
            public string Status { get; set; }
        }

        public class RestQuery
        {
            public string Condition { get; set; }
            public string Condition2 { get; set; }            
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