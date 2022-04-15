using System;
using System.Threading;
using System.Threading.Tasks;

using Mapster;

using MediatR;

using SqlSugar;

using UserCenter.Commands;
using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using MF.Core.Extensions;
using MF.FluentValidation;
using MF.NetCoreApp;
using MF.Orm;

namespace UserCenter.CommandHandles
{
    public class AccessLogCommandHandle : ICommandHandler,
        IRequestHandler<CreateAccessLogCommand, PubResponse>,
        IRequestHandler<UpdateAccessLogCommand, PubResponse>,
        IRequestHandler<QueryPageAccessLogCommand, PubResponse>,
        IRequestHandler<DeleteAccessLogCommand, PubResponse>
    {
        private readonly IAccesslogRepository _accessLogRepository;
        private readonly GlobalCore _globalCore;

        public AccessLogCommandHandle(GlobalCore globalCore, IAccesslogRepository accessLogRepository)
        {
            _globalCore = globalCore;
            _accessLogRepository = accessLogRepository;
        }

        public Task<PubResponse> Handle(CreateAccessLogCommand request, CancellationToken cancellationToken)
        {
            var destObject = request.Adapt<CreateAccessLogCommand, Accesslog>();
            _accessLogRepository.Insert(destObject);
            return Succeed();
        }

        public Task<PubResponse> Handle(UpdateAccessLogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(QueryPageAccessLogCommand cmd, CancellationToken cancellationToken)
        {
            int total = 0;

            var page = _accessLogRepository.Queryable()
                .WhereIF(cmd.Condition.NotNull(), cmd.Condition)
                .OrderBy((o) => o.CreateTime, OrderByType.Desc)
                .ToPageList(cmd.PageNum, cmd.PageSize, ref total);
            return Succeed(new { list = page, total });
        }

        public Task<PubResponse> Handle(DeleteAccessLogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}