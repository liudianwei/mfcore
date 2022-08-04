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
    public class AccessLogCommandHandle : ICommandHandler,
        IRequestHandler<QueryPageAccessLogCommandV2, PubResponse>
    {
        private readonly IAccesslogRepository _accessLogRepository;
        private readonly GlobalCore _globalCore;
        public AccessLogCommandHandle(GlobalCore globalCore, IAccesslogRepository accessLogRepository)
        {
            _globalCore = globalCore;
            _accessLogRepository = accessLogRepository;
        }

        public Task<PubResponse> Handle(QueryPageAccessLogCommandV2 cmd, CancellationToken cancellationToken)
        {
            int totalCount = 0;
            var list = _accessLogRepository.Queryable()
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