using MF.Rest;

using NLog;

using Quartz;

using System;
using System.Threading.Tasks;

namespace MF.ExportTask
{
    [DisallowConcurrentExecution]
    public sealed class Run : IJob
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
            var token = context.JobDetail.JobDataMap.Get("Token").ToString();
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
                //查询未完成的导出任务
                string result = MRestClient.Post(uri, resource);

                Console.WriteLine(result);
                //判断是否有正在执行中的
                //没有就取第一个去执行

                //执行任务
                result = MRestClient.Post(uri, resource, jobargs, token);

                //switch (method)
                //{
                //    case "get":
                //        result = MRestClient.Get(uri, resource + jobargs, 5000, token);
                //        break;

                //    case "post":
                //        result = MRestClient.Post(uri, resource, jobargs, token);
                //        break;

                //    case "delete":
                //        result = MRestClient.Delete(uri, resource, null, token);
                //        break;

                //    default:
                //        break;
                //}

                if (result.Contains("\"status\":\"success\""))
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
    }
}