using MF.Rest;

using Newtonsoft.Json;

using NLog;

using Quartz;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch sw_qs = new Stopwatch();
            sw_qs.Start();//开始计时
            //Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Execute begin Ver." + Ver.ToString());
            //Console.WriteLine($"[{DateTime.Now}] " + "Job_" + context.JobDetail.JobDataMap.Get("JobName").ToString() + " Executing ...");
            var uri = context.JobDetail.JobDataMap.Get("Target").ToString();
            var resource = context.JobDetail.JobDataMap.Get("TargetDetail").ToString();
            var name = context.JobDetail.JobDataMap.Get("JobName").ToString();

            try
            {
                //调用删除接口 删除N天之前的Excel和导出记录
                string resultstr = MRestClient.Delete(uri, resource);
                var result = JsonConvert.DeserializeObject<RestResult>(resultstr);

                if (!result.Status.ToLower().Equals("success"))
                {
                    logger.Error($"### JobName:{name} {uri}{resource} === {result} ###");
                }
                await Task.Delay(500);
            }
            catch (Exception e)
            {
                logger.Error($"### {e.Source} === {e.Message} ###");
            }
            sw_qs.Stop();//结束计时
            Console.WriteLine($"[{DateTime.Now}] Job_{context.JobDetail.JobDataMap.Get("JobName").ToString()} Execute Complete,Time consuming {sw_qs.ElapsedMilliseconds}ms ");
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