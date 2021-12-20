using System;
using System.Threading;
using System.Threading.Tasks;

using MF.Job.Core.Services;

using Quartz;

namespace MF.Job.Core
{
    public class SchedulerJobListener : IJobListener
    {
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            DateTime NextFireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local);
            DateTime FireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.DateTime, TimeZoneInfo.Local);

            double TotalSeconds = context.JobRunTime.TotalSeconds;
            string JobName = string.Empty;
            string LogContent = string.Empty;
            if (context.MergedJobDataMap != null)
            {
                JobName = context.MergedJobDataMap.GetString("JobName");
                System.Text.StringBuilder log = new System.Text.StringBuilder();
                int i = 0;
                foreach (var item in context.MergedJobDataMap)
                {
                    string key = item.Key;
                    if (key.StartsWith("extend_"))
                    {
                        if (i > 0)
                        {
                            log.Append(",");
                        }
                        log.AppendFormat("{0}:{1}", item.Key, item.Value);
                        i++;
                    }
                }
                if (i > 0)
                {
                    LogContent = string.Concat("[", log.ToString(), "]");
                }
            }
            if (jobException != null)
            {
                LogContent = LogContent + " EX:" + jobException.ToString();
            }
            new BackgroundJobService().UpdateBackgroundJobStatus(context.JobDetail.Key.Name, JobName, FireTimeUtc, NextFireTimeUtc, TotalSeconds, LogContent);
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public string Name
        {
            get { return "SchedulerJobListener"; }
        }
    }
}