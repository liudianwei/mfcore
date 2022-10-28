using Mapster;

using MediatR;

using MF.Core.Extensions;
using MF.FluentValidation;
using MF.Job.Core.Business.Info;
using MF.Job.Core.Services;
using MF.NetCore;
using MF.NetCoreApp;
using MF.Orm;
using MF.Orm.UnitOfWork;
using MF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UserCenter.Commands.JobTask;
using UserCenter.Enums;

namespace UserCenter.CommandHandles
{
    public class JobTaskCommandHandle : ICommandHandler,
        IRequestHandler<CreateJobTaskCommand, PubResponse>,
        IRequestHandler<DeleteJobTaskCommand, PubResponse>,
        IRequestHandler<UpdateJobTaskCommand, PubResponse>,
        IRequestHandler<QueryJobTaskCommand, PubResponse>,
        IRequestHandler<QueryListJobTaskCommand, PubResponse>,
        IRequestHandler<QueryPageJobTaskCommand, PubResponse>,
        IRequestHandler<QueryAllJobTaskCommand, PubResponse>,
        IRequestHandler<SetStateJobTaskCommand, PubResponse>,
        IRequestHandler<BatchSetStateJobTaskCommand, PubResponse>

    {
        private readonly GlobalCore _globalCore;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BackgroundJobService _backgroundJobService = new BackgroundJobService();

        //public JobTaskCommandHandle(
        //    BackgroundJobService backgroundJobService
        //    , GlobalCore globalCore
        //    , IUnitOfWork unitOfWork)
        //{
        //    _backgroundJobService = backgroundJobService;
        //    _globalCore = globalCore;
        //    _unitOfWork = unitOfWork;
        //}

        public JobTaskCommandHandle(
           GlobalCore globalCore
           , IUnitOfWork unitOfWork)
        {
            _globalCore = globalCore;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 创建 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(CreateJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Name.IsNull() || cmd.Target.IsNull() || cmd.TargetDetail.IsNull() || cmd.Method.IsNull() || cmd.CronExpression.IsNull())
            {
                return Failed(BaseSystemError.PARAM_IS_ERROR);
            }
            // Name 不能重复
            if (_backgroundJobService.IsExistByName(cmd.Name))
            {
                return Failed(BaseSystemError.DATA_ALREAD_EXISTS);
            }

            JobTask jobTask = cmd.Adapt<JobTask>();
            jobTask.State = 0;
            jobTask.CreatedByUserId = _globalCore.UserId;
            jobTask.CreatedByUserName = _globalCore.UserName;
            jobTask.CreatedDateTime = DateTime.Now;
            jobTask.LastUpdatedByUserId = _globalCore.UserId;
            jobTask.LastUpdatedByUserName = _globalCore.UserName;
            jobTask.LastUpdatedDateTime = DateTime.Now;
            jobTask.ErrorMsg = "";
            jobTask.BackgroundJobId = Guid.NewGuid().ToString();
            _backgroundJobService.InsertBackgroundJob(jobTask);
            return Succeed(jobTask.BackgroundJobId);
        }

        /// <summary>
        /// 删除 （单个或批量）
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(DeleteJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            return SucceedOrFail(_backgroundJobService.DeleteBackgroundJob(cmd.List.First()));
        }

        /// <summary>
        /// 更新 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(UpdateJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Name.IsNull() || cmd.Target.IsNull() || cmd.TargetDetail.IsNull() || cmd.Method.IsNull() || cmd.CronExpression.IsNull())
            {
                return Failed(BaseSystemError.PARAM_IS_ERROR);
            }
            // Name 不能重复
            if (_backgroundJobService.IsExistByName(cmd.Name))
            {
                return Failed(BaseSystemError.DATA_ALREAD_EXISTS);
            }
            JobTask jobTask = _backgroundJobService.GetBackgroundJobInfo(cmd.BackgroundJobId);

            // 对象没有找到
            if (jobTask.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            var updateObj = cmd.Adapt<JobTask>();

            var flag = _backgroundJobService.UpdateBackgroundJob(updateObj);
            return SucceedOrFail(flag);
        }

        /// <summary>
        /// 根据Id查询 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.BackgroundJobId.NotNull())
            {
                var jobTask = _backgroundJobService.GetBackgroundJobInfo(cmd.BackgroundJobId);
                cmd = jobTask.Adapt<QueryJobTaskCommand>();
            }
            return Succeed(cmd);
        }

        /// <summary>
        /// 根据Id列表查询 列表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryListJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            var list = new List<QueryAllJobTaskCommand>();
            if (cmd.List.NotNull())
            {
                var jobTasks = _backgroundJobService.GeByIDsScheduleJobInfoList(cmd.List);
                list = jobTasks.Adapt<List<QueryAllJobTaskCommand>>();
            }
            return Succeed(list);
        }

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPageJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            System.Collections.Concurrent.ConcurrentDictionary<string, string> dictionary = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();
            dictionary.TryAdd("JobType", cmd.JobType);
            dictionary.TryAdd("Name", cmd.Name);
            dictionary.TryAdd("State", cmd.State);
            var pages = _backgroundJobService.GeBackgroundJobInfoPagerList(new PageParameter(cmd.PageSize, cmd.PageNum, dictionary));

            return Succeed(new { List = pages.dataList, total = pages.TotalRecord });
        }

        /// <summary>
        /// 查询所有 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryAllJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            var list = _backgroundJobService.GeAllScheduleJobInfoList();
            var all = list.Adapt<List<QueryAllJobTaskCommand>>();
            return Succeed(all);
        }

        /// <summary>
        /// 修改 job运行状态
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(SetStateJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.State==2)
            {
                return Failed("状态不允许修改为删除，只能调用删除接口进行删除");
            }

            // 前端要求停止 设置为5job才情进行停止操作
            if (cmd.State == (int)JobTaskEnum.PopStop)
            {
                cmd.State = (int)JobTaskEnum.PopProceed;
            }
            if (cmd.State == (int)JobTaskEnum.PopFinished)
            {
                cmd.State = (int)JobTaskEnum.PopWait;
            }
            if (cmd.BackgroundJobId.NotNull())
            {
                var jobTask = _backgroundJobService.GetBackgroundJobInfo(cmd.BackgroundJobId);

                if (jobTask.NotNull())
                {

                    var flag = _backgroundJobService.UpdateBackgroundJobState(jobTask.BackgroundJobId, cmd.State);
                    return SucceedOrFail(flag);

                }
                else
                {
                    return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }
            }
            return Succeed();
        }

        /// <summary>
        /// 批量修改  job运行状态
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Transaction]
        public Task<PubResponse> Handle(BatchSetStateJobTaskCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.List.NotNullT())
            {
                cmd.List = cmd.List.Distinct().ToList();
                var jobTasks = _backgroundJobService.GeByIDsScheduleJobInfoList(cmd.List);
                if (jobTasks.Count != cmd.List.Count)
                {
                    ThrowError(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }

                if (jobTasks.NotNullT())
                {
                    foreach (var item in jobTasks)
                    {
                        _backgroundJobService.UpdateBackgroundJobState(item.BackgroundJobId, cmd.State);
                    }
                }
                else
                {
                    ThrowError(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }
            }
            return Succeed();
        }

    }
}