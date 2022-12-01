using System;
using System.Collections.Generic;

using MF.Job.Core.Business.Info;

using SqlSugar;

namespace MF.Job.Core.Business.Manager
{
    public class BackgroundJobManager : BaseManager
    {
        #region BackgroundJobInfo

        /// <summary>
        /// Job新增
        /// </summary>
        /// <param name="backgroundJobInfo">BackgroundJobInfo实体</param>
        /// <returns></returns>
        public bool InsertBackgroundJob(JobTask backgroundJobInfo)
        {
            backgroundJobInfo.CreatedDateTime = DateTime.Now;
            backgroundJobInfo.LastUpdatedDateTime = DateTime.Now;
            return db.Insertable(backgroundJobInfo).ExecuteCommand() > 0;
        }

        /// <summary>
        /// Job修改
        /// </summary>
        /// <param name="backgroundJobInfo">BackgroundJobInfo实体</param>
        /// <returns></returns>
        public bool UpdateBackgroundJob(JobTask JobInfo)
        {
            JobInfo.LastUpdatedDateTime = DateTime.Now;
            db.Updateable(JobInfo).IgnoreColumns(it => new { it.LastRunTime, it.NextRunTime, it.RunCount, it.CreatedByUserId, it.CreatedByUserName, it.CreatedDateTime }).Where(it => it.BackgroundJobId == JobInfo.BackgroundJobId).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// Job删除
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <returns></returns>
        public bool DeleteBackgroundJob(string BackgroundJobId)
        {
            JobTask backgroundJobInfo = new JobTask
            {
                BackgroundJobId = BackgroundJobId,
                State = 2,
                LastUpdatedDateTime = DateTime.Now
            };
            db.Updateable(backgroundJobInfo).UpdateColumns(it => new { it.State, it.LastUpdatedDateTime })
                .Where(it => it.BackgroundJobId == BackgroundJobId.ToString())
                .Where(it => it.Name != "ExportExcel" && it.Name != "DeleteExcel")
                .ExecuteCommand();
            return true;
        }

        /// <summary>
        /// Job详情
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <returns></returns>
        public JobTask GetBackgroundJobInfo(string BackgroundJobId)
        {
            return db.Queryable<JobTask>().Where(it => it.BackgroundJobId == BackgroundJobId && it.State != 2).First();
        }


        /// <summary>
        /// 根据id列表获取Job详情
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <returns></returns>
        public List<JobTask> GeByIDsScheduleJobInfoList(List<string> BackgroundJobIds)
        {
            return db.Queryable<JobTask>().Where(it => BackgroundJobIds.Contains(it.BackgroundJobId) && it.State != 2).ToList();
        }

        /// <summary>
        /// Job集合(分页) //State 0停止 1运行 2删除 3启动中 5停止中
        /// </summary>
        /// <param name="parameter">参数集</param>
        /// <returns></returns>
        public PagerModel<JobTask> GeBackgroundJobInfoPagerList(PageParameter parameter)
        {
            int TotalRecord = 0;
            List<JobTask> dataList = null;
            string name = parameter.GetParameter("Name");
            string state = parameter.GetParameter("State");
            string jobType = parameter.GetParameter("JobType");
            dataList = db.Queryable<JobTask>()
                     .Where(it => it.State != 2)
                     .WhereIF(!string.IsNullOrWhiteSpace(name), it => it.Name.Contains(name))
                     .WhereIF(!string.IsNullOrWhiteSpace(state), it => it.State.Equals(state))
                     .WhereIF(!string.IsNullOrWhiteSpace(jobType), it => it.JobType.Equals(jobType))
                     .OrderBy(it => it.JobType, OrderByType.Desc)
                     .ToPageList(parameter.currentPageIndex, parameter.rows, ref TotalRecord);

            PagerModel<JobTask> pagerModel = new PagerModel<JobTask>
            {
                dataList = dataList,
                TotalRecord = TotalRecord,
                CurrentPage = parameter.currentPageIndex
            };
            pagerModel.CalculateTotalPage(parameter.rows, pagerModel.TotalRecord);
            return pagerModel;
        }

        /// <summary>
        /// 获取允许调度的Job集合
        /// </summary>
        /// <returns></returns>
        public List<JobTask> GeAllowScheduleJobInfoList()
        {
            List<JobTask> list = null;
            list = db.Queryable<JobTask>()
                .Where(it => it.State == 1 || it.State == 3 || it.State == 5)//0停止 1运行 3启动中 5停止中
                .OrderBy(it => it.CreatedDateTime, OrderByType.Desc)
                .ToList();
            return list;
        }

        /// <summary>
        /// 获取所有的Job集合
        /// </summary>
        /// <returns></returns>
        public List<JobTask> GeAllScheduleJobInfoList()
        {
            List<JobTask> list = null;
            list = db.Queryable<JobTask>()
                .OrderBy(it => it.CreatedDateTime, OrderByType.Desc)
                .ToList();
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
            //if (State == 5 || State == 3 || State == 0)
            //{
            //    Console.WriteLine(State);
            //}
            db.Updateable<JobTask>().SetColumns(it => it.State == State)
           .Where(it => it.BackgroundJobId == BackgroundJobId)
           .ExecuteCommand();
            return true;
        }

        /// <summary>
        /// 更新Job状态 批量
        /// </summary>
        /// <param name="BackgroundJobIds"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool UpdateJobStateByIds(List<string> BackgroundJobIds, int State)
        {
            var rows = db.Updateable<JobTask>().SetColumns(it => it.State == State)
           .Where(it => BackgroundJobIds.Contains(it.BackgroundJobId))
           .ExecuteCommand();
            return rows > 0 ? true : false;
        }

        /// <summary>
        /// 更新Job运行信息
        /// </summary>
        /// <param name="BackgroundJobId">Job ID</param>
        /// <param name="LastRunTime">最后运行时间</param>
        /// <param name="NextRunTime">下次运行时间</param>
        public void UpdateBackgroundJobStatus(string BackgroundJobId, DateTime LastRunTime, DateTime NextRunTime)
        {
            db.Updateable<JobTask>()
                .SetColumns(it => it.RunCount == (it.RunCount + 1))
                .SetColumns(it => it.LastRunTime == LastRunTime)
                .SetColumns(it => it.NextRunTime == NextRunTime)
                .Where(it => it.BackgroundJobId == BackgroundJobId)
                .ExecuteCommand();
        }

        /// <summary>
        /// 根据name检查重复项
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public bool IsExistByName(string jobName, string backgroundJobId = "")
        {
            var blFlag = db.Queryable<JobTask>()
                .Where(it => it.State != 2 && it.Name.Contains(jobName))
                .WhereIF(backgroundJobId != "", it => it.BackgroundJobId!= backgroundJobId)
                .Any();
            return blFlag;
        }

        #endregion BackgroundJobInfo
    }
}