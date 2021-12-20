using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using MF.Job.Core.Business.Info;
using MF.Job.Core.Services;
using MF.Job.JobItems;

using NLog;

using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace MF.Job.Core
{
    public class QuartzManager
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 从程序集中加载指定类
        /// </summary>
        /// <param name="assemblyName">含后缀的程序集名</param>
        /// <param name="className">含命名空间完整类名</param>
        /// <returns></returns>
        private Type GetClassInfo(string dir, string assemblyName, string className)
        {
            Type type = null;
            try
            {
                assemblyName = GetAbsolutePath(assemblyName, dir);
                Assembly assembly = null;
                assembly = Assembly.LoadFrom(assemblyName);
                type = assembly.GetType(className, true, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Trace(ex.StackTrace);
            }
            return type;
        }

        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        ///  获取文件的绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        public string GetAbsolutePath(string relativePath, string dir = "")
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            if (dir.Equals("outerDLL"))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir, relativePath);
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }

        /// <summary>
        /// Job调度
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="jobInfo"></param>
        public void ScheduleJob(IScheduler scheduler, JobTask jobInfo)
        {
            Type type = null;

            switch (jobInfo.JobType)
            {
                case "innerDLL":
                case "outerDLL":
                    type = GetClassInfo(jobInfo.JobType, jobInfo.Target, jobInfo.TargetDetail);
                    break;

                case "API":
                    switch (jobInfo.Name)
                    {
                        case "ExportExcel":
                            type = new ExportExcelJob().GetType();
                            break;
                        case "DeleteExcel":
                            type = new DeleteExcelJob().GetType();
                            break;
                        default:
                            type = GetClassInfo(jobInfo.JobType, "MF.ApiJob.dll", "ApiJob.Run");
                            break;
                    }
                    break;

                default:
                    break;
            }

            if (type != null)
            {
                IJobDetail job = new JobDetailImpl(jobInfo.BackgroundJobId.ToString(), jobInfo.BackgroundJobId.ToString() + "Group", type);
                job.JobDataMap.Add("JobName", jobInfo.Name);
                job.JobDataMap.Add("Target", jobInfo.Target);
                job.JobDataMap.Add("TargetDetail", jobInfo.TargetDetail);
                job.JobDataMap.Add("Token", jobInfo.Token);
                job.JobDataMap.Add("Method", jobInfo.Method);
                job.JobDataMap.Add("JobArgs", jobInfo.JobArgs);

                if (ValidExpression(jobInfo.CronExpression))
                {
                    CronTriggerImpl trigger = new CronTriggerImpl
                    {
                        CronExpressionString = jobInfo.CronExpression,
                        Name = jobInfo.BackgroundJobId.ToString(),
                        Description = jobInfo.Description,
                        StartTimeUtc = DateTime.Now,
                        Group = jobInfo.BackgroundJobId + "TriggerGroup"
                    };
                    scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    //TODO 执行其他时效的job
                    Console.WriteLine($"[{DateTime.Now}]" + jobInfo.Name + ": " + jobInfo.CronExpression + "不是正确的Cron表达式,无法启动该任务");
                    logger.Warn(jobInfo.BackgroundJobId + ": " + jobInfo.Name + ": " + jobInfo.CronExpression + "不是正确的Cron表达式,无法启动该任务");
                }
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now}]" + jobInfo.Name + ": 没有找到Job dll");
                logger.Warn(jobInfo.BackgroundJobId + ": " + jobInfo.Name + ": 没有找到Job dll");
            }
        }

        /// <summary>
        /// 启动job
        /// </summary>
        /// <param name="Scheduler"></param>
        public async Task StartJob(IScheduler Scheduler, JobTask jobInfo)
        {
            JobKey jobKey = new JobKey(jobInfo.BackgroundJobId.ToString(), jobInfo.BackgroundJobId.ToString() + "Group");
            if (await Scheduler.CheckExists(jobKey) == false)//判断job不存在
            {
                if (jobInfo.State == 1 || jobInfo.State == 3)
                {
                    ScheduleJob(Scheduler, jobInfo);//启动job

                    if (await Scheduler.CheckExists(jobKey) == false)//判断是否启动成功
                    {
                        new BackgroundJobService().UpdateBackgroundJobState(jobInfo.BackgroundJobId, 0);
                    }
                    else
                    {
                        new BackgroundJobService().UpdateBackgroundJobState(jobInfo.BackgroundJobId, 1);
                    }
                }
                else if (jobInfo.State == 5)
                {
                    new BackgroundJobService().UpdateBackgroundJobState(jobInfo.BackgroundJobId, 0);
                }
            }
            else
            {
                if (jobInfo.State == 5)//被标记为停止中的job改成停止
                {
                    await Scheduler.DeleteJob(jobKey);//删除Job
                    new BackgroundJobService().UpdateBackgroundJobState(jobInfo.BackgroundJobId, 0);
                }
                else if (jobInfo.State == 3)
                {
                    new BackgroundJobService().UpdateBackgroundJobState(jobInfo.BackgroundJobId, 1);
                }
            }
        }

        /// <summary>
        /// Job状态管控
        /// </summary>
        /// <param name="Scheduler"></param>
        public async Task JobScheduler(IScheduler Scheduler)
        {
            // 读取数据库job列表
            List<JobTask> list = new BackgroundJobService().GeAllowScheduleJobInfoList();

            if (list != null && list.Count > 0)
            {
                //循环执行job
                foreach (JobTask jobInfo in list)
                {
                    await StartJob(Scheduler, jobInfo);
                }
            }
        }
    }
}