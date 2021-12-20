using System;
using MF.Job.Core;
using MF.Job.Core.Business.Info;
using Microsoft.Extensions.Configuration;
using NLog;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace MF.Job
{
    public class ServiceRunner
    {
        private readonly IScheduler scheduler;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ServiceRunner()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
        }

        public bool Start(IConfiguration configuration)
        {
            scheduler.ListenerManager.AddJobListener(new SchedulerJobListener(), GroupMatcher<JobKey>.AnyGroup());
            scheduler.Start();

            try
            {
                var assemblyName = "MF.Job.dll";
                var className = "MF.Job.JobItems.ManagerJob";
                var jobArgs = "";
                var name = "ManagerJob";
                var des = "Main Job Manage";

                if (!bool.TryParse(configuration["Jobs:Main:Enabled"], out var state))
                {
                    Console.WriteLine("Jobs:Main:Enabled 配置不正确: ");
                    logger.Error("Jobs:Main:Enabled 配置不正确: ");
                    return false;
                }

                var cronExpression = configuration["Jobs:Main:CronExpression"];
                if (cronExpression == null || cronExpression == "")
                {
                    Console.WriteLine("Jobs:Main:CronExpression 配置不正确: ");
                    logger.Error("Jobs:Main:CronExpression 配置不正确: ");
                    return false;
                }

                JobTask jobInfo = new JobTask
                {
                    BackgroundJobId = "EC464D8F-D873-4393-BED1-70B3219C2BB2",
                    State = state ? 1 : 0,
                    CronExpression = cronExpression,
                    Target = assemblyName,
                    Description = des,
                    TargetDetail = className,
                    JobArgs = jobArgs,
                    Name = name,
                    JobType = "innerDLL"
                };

                if (state)
                {
                    Console.WriteLine("Job Start");
                    logger.Info("Job Start");
                    _ = new QuartzManager().StartJob(scheduler, jobInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Jobs配置不正确: " + e.Message);
                logger.Trace("Jobs配置不正确: " + e.StackTrace);
                return false;
            }
            return true;
        }

        public bool Stop()
        {
            scheduler.Shutdown(false);
            Console.WriteLine("Job Stop");
            logger.Info("Job Stop");
            return true;
        }

        public bool Continue()
        {
            scheduler.ResumeAll();
            Console.WriteLine("Job Continue");
            logger.Info("Job Continue");
            return true;
        }

        public bool Pause()
        {
            scheduler.PauseAll();
            Console.WriteLine("Job Pause");
            logger.Info("Job Pause");
            return true;
        }
    }
}