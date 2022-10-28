using System;
using System.Collections.Generic;

using MF.Job.Core.Business.Info;
using MF.Job.Core.Business.Manager;

namespace MF.Job.Core.Services
{
    public class BackgroundJobService
    {
        public BackgroundJobService()
        {
        }

        /// <summary>
        /// Job新增
        /// </summary>
        /// <param name="backgroundJobInfo">BackgroundJobInfo实体</param>
        /// <returns></returns>
        public bool InsertBackgroundJob(JobTask backgroundJobInfo)
        {
            return new BackgroundJobManager().InsertBackgroundJob(backgroundJobInfo);
        }

        /// <summary>
        /// Job修改
        /// </summary>
        /// <param name="backgroundJobInfo">BackgroundJobInfo实体</param>
        /// <returns></returns>
        public bool UpdateBackgroundJob(JobTask backgroundJobInfo)
        {
            return new BackgroundJobManager().UpdateBackgroundJob(backgroundJobInfo);
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <returns></returns>
        public bool DeleteBackgroundJob(string BackgroundJobId)
        {
            return new BackgroundJobManager().DeleteBackgroundJob(BackgroundJobId);
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="idList">ID集合</param>
        /// <returns></returns>
        public bool DeleteBackgroundJob(List<string> idList, out string rtMsg)
        {
            rtMsg = string.Empty;
            int i = 0;
            if (idList != null && idList.Count > 0)
            {
                foreach (string BackgroundJobId in idList)
                {
                    JobTask backgroundJobInfo = GetBackgroundJobInfo(BackgroundJobId);
                    if (backgroundJobInfo.State != 0)
                    {
                        rtMsg = string.Format("{0}状态不为 停止状态,无法进行删除！", backgroundJobInfo.Name);
                        return false;
                    }
                }

                foreach (string BackgroundJobId in idList)
                {
                    DeleteBackgroundJob(BackgroundJobId);
                    i++;
                }
            }
            bool result = i > 0;
            return result;
        }

        /// <summary>
        /// Job详情
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <returns></returns>
        public JobTask GetBackgroundJobInfo(string BackgroundJobId)
        {
            return new BackgroundJobManager().GetBackgroundJobInfo(BackgroundJobId);
        }

        /// <summary>
        /// Job详情
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <returns></returns>
        public List<JobTask> GeByIDsScheduleJobInfoList(List<string> BackgroundJobIds)
        {
            return new BackgroundJobManager().GeByIDsScheduleJobInfoList(BackgroundJobIds);
        }

        /// <summary>
        /// Job集合(分页 )
        /// </summary>
        /// <param name="parameter">参数集</param>
        /// <returns></returns>
        public PagerModel<JobTask> GeBackgroundJobInfoPagerList(PageParameter parameter)
        {
            return new BackgroundJobManager().GeBackgroundJobInfoPagerList(parameter);
        }

        /// <summary>
        /// 获取允许调度的Job集合
        /// </summary>
        /// <returns></returns>
        public List<JobTask> GeAllowScheduleJobInfoList()
        {
            var list = new BackgroundJobManager().GeAllowScheduleJobInfoList();
            return list;
        }

        /// <summary>
        /// 获取所有的Job集合
        /// </summary>
        /// <returns></returns>
        public List<JobTask> GeAllScheduleJobInfoList()
        {
            var list = new BackgroundJobManager().GeAllScheduleJobInfoList();
            return list;
        }

        /// <summary>
        /// 更新Job状态
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <param name="State">状态</param>
        /// <returns></returns>
        public bool UpdateBackgroundJobState(string BackgroundJobId, int State)
        {
            return new BackgroundJobManager().UpdateBackgroundJobState(BackgroundJobId, State);
        }


        /// <summary>
        /// 更新Job运行信息
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <param name="LastRunTime">最后运行时间</param>
        /// <param name="NextRunTime">下次运行时间</param>
        public void UpdateBackgroundJobStatus(string BackgroundJobId, DateTime LastRunTime, DateTime NextRunTime)
        {
            new BackgroundJobManager().UpdateBackgroundJobStatus(BackgroundJobId, LastRunTime, NextRunTime);
        }

        /// <summary>
        /// 更新Job运行信息
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <param name="JobName">Job名称</param>
        /// <param name="LastRunTime">最后运行时间</param>
        /// <param name="NextRunTime">下次运行时间</param>
        /// <param name="ExecutionDuration">运行时长</param>
        /// <param name="RunLog">日志</param>
        public void UpdateBackgroundJobStatus(string BackgroundJobId, string JobName, DateTime LastRunTime, DateTime NextRunTime, double ExecutionDuration, string RunLog)
        {
            UpdateBackgroundJobStatus(BackgroundJobId, LastRunTime, NextRunTime);
        }

        /// <summary>
        /// 根据name检查重复项
        /// </summary>
        /// <returns></returns>
        public bool IsExistByName(string name)
        {
            return new BackgroundJobManager().IsExistByName(name);
        }

    }
}