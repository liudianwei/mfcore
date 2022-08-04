using DAL.UserCenter.IRepository;
using MediatR;
using MF.Core.Extensions;
using MF.FluentValidation;
using MF.NetCoreApp;
using MF.Orm;
using SqlSugar;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserCenter.Commands;

namespace UserCenter.CommandHandles.v2
{
    public class OperationLogCommandHandle : ICommandHandler,
        IRequestHandler<QueryPageOperationLogCommandV2, PubResponse>
    {
        private readonly IOperationlogRepository _operationLogRepository;
        private readonly GlobalCore _globalCore;
        public OperationLogCommandHandle(GlobalCore globalCore, IOperationlogRepository operationLogRepository)
        {
            _globalCore = globalCore;
            _operationLogRepository = operationLogRepository;
        }

        public Task<PubResponse> Handle(QueryPageOperationLogCommandV2 cmd, CancellationToken cancellationToken)
        {
            int totalCount = 0;
            var list = _operationLogRepository.Queryable()
                .WhereIF(cmd.Condition.NotNull(), cmd.Condition)
                .OrderBy((o) => o.CreateTime, OrderByType.Desc)
                .Skip((cmd.PageNum - 1) * cmd.PageSize)
                .Take(cmd.PageSize * 2).ToList();

            if (list != null)
            {
                var count = Convert.ToInt32(list.Count);
                if (count > (cmd.PageSize + 1))
                {
                    totalCount = (cmd.PageSize * cmd.PageNum) + 1;
                }
                else
                {
                    totalCount = ((cmd.PageNum - 1) * cmd.PageSize + count);
                }
            }

            list = list.Take(cmd.PageSize).ToList();
            return Succeed(new { List = list, total = totalCount });
        }
    }
}