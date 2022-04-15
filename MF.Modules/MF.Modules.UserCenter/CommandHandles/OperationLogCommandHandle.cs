using System;
using System.Threading;
using System.Threading.Tasks;

using Common.Commands;

using Mapster;

using MediatR;

using Microsoft.Extensions.Localization;

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
    public class OperationLogCommandHandle : ICommandHandler,
        IRequestHandler<CreateOperationLogCommand, PubResponse>,
        IRequestHandler<UpdateOperationLogCommand, PubResponse>,
        IRequestHandler<QueryPageOperationLogCommand, PubResponse>,
        IRequestHandler<DeleteOperationLogCommand, PubResponse>
    {
        private readonly IOperationlogRepository _operationLogRepository;
        private readonly GlobalCore _globalCore;

        public OperationLogCommandHandle(GlobalCore globalCore, IOperationlogRepository operationLogRepository)
        {
            _globalCore = globalCore;
            _operationLogRepository = operationLogRepository;
        }

        public Task<PubResponse> Handle(CreateOperationLogCommand request, CancellationToken cancellationToken)
        {
            var destObject = request.Adapt<Operationlog>();
            bool res = _operationLogRepository.Insert(destObject);
            return SucceedOrFail(res);
        }

        public Task<PubResponse> Handle(UpdateOperationLogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(QueryPageOperationLogCommand cmd, CancellationToken cancellationToken)
        {
            int total = 0;
            var page = _operationLogRepository.Queryable()
                .WhereIF(cmd.Condition.NotNull(), cmd.Condition)
                .OrderBy((o) => o.CreateTime, OrderByType.Desc)
                .ToPageList(cmd.PageNum, cmd.PageSize, ref total);
            return Succeed(new { list = page, total });
        }

        public Task<PubResponse> Handle(DeleteOperationLogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}