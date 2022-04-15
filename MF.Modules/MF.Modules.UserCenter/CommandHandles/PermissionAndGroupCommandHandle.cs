using System;
using System.Threading;
using System.Threading.Tasks;

using Mapster;

using MediatR;

using UserCenter.Commands;
using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using MF.Core.Extensions;
using MF.FluentValidation;
using MF.NetCoreApp;
using MF.Orm;

namespace UserCenter.CommandHandles
{
    public class PermissionAndGroupCommandHandle : ICommandHandler,
        IRequestHandler<CreatePermissionAndGroupCommand, PubResponse>,
        IRequestHandler<UpdatePermissionAndGroupCommand, PubResponse>,
        IRequestHandler<QueryPermissionAndGroupCommand, PubResponse>,
        IRequestHandler<DeletePermissionAndGroupCommand, PubResponse>
    {
        private readonly IPermissionPermissiongroupRepository _Repository;
        private readonly GlobalCore _globalCore;

        public PermissionAndGroupCommandHandle(GlobalCore globalCore, IPermissionPermissiongroupRepository Repository)
        {
            _globalCore = globalCore;
            _Repository = Repository;
        }

        public Task<PubResponse> Handle(CreatePermissionAndGroupCommand request, CancellationToken cancellationToken)
        {
            var permissionPermissiongroup = request.Adapt<PermissionPermissiongroup>();
            var flag = _Repository.Insert(permissionPermissiongroup);
            return SucceedOrFail(flag);
        }

        public Task<PubResponse> Handle(UpdatePermissionAndGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(QueryPermissionAndGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(DeletePermissionAndGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}