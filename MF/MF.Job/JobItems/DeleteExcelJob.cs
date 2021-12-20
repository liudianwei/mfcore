using MF.Rest;

using Newtonsoft.Json;

using NLog;

using Quartz;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MF.Job.JobItems
{
    [DisallowConcurrentExecution]
    public sealed class DeleteExcelJob : IJob
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async Task Execute(IJobExecutionContext context)
        {
            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute begin Ver." + Ver.ToString());
            logger.Info("Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute begin Ver." + Ver.ToString());
            Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Executing ...");
            logger.Info("Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Executing ...");

            var uri = context.JobDetail.JobDataMap.Get("Target").ToString();
            var resource = context.JobDetail.JobDataMap.Get("TargetDetail").ToString();
            //var token = context.JobDetail.JobDataMap.Get("Token").ToString();
            //var method = context.JobDetail.JobDataMap.Get("Method").ToString();
            var jobargs = context.JobDetail.JobDataMap.Get("JobArgs").ToString();
            var name = context.JobDetail.JobDataMap.Get("JobName").ToString();
            string head = $"JobId: {context.JobDetail.Key.Name}" + Environment.NewLine
                + $"JobName: {name}" + Environment.NewLine
                + $"TotalSeconds: {context.JobRunTime.TotalSeconds}(s)" + Environment.NewLine
                + $"FireTime: {TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.DateTime, TimeZoneInfo.Local)}" + Environment.NewLine
                + $"NextFireTime: {TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local)}" + Environment.NewLine
                + $"Message: " + Environment.NewLine;

            try
            {
                //调用删除接口 删除N天之前的Excel和导出记录
                string resultstr = MRestClient.Post(uri, resource, jobargs);
                var result = JsonConvert.DeserializeObject<RestResult>(resultstr);

                if (result.Status.ToLower().Equals("success"))
                {
                    logger.Info(head + result);
                }
                else
                {
                    logger.Error(head + result);
                }

                await Task.Delay(500);
            }
            catch (Exception e)
            {
                logger.Trace(head + e.StackTrace);
            }
            finally
            {
                Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute end ");
                logger.Info("Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute end ");
            }
        }

        public class ExportLogDto
        {
            public string Id { get; set; }
        }

        public class RestResult
        {
            public string Code { get; set; }
            public List<ExportLogDto> Data { get; set; }
            public string Message { get; set; }
            public string Status { get; set; }
        }
    }
}