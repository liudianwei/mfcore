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
    public class RolePermissionCommandHandle : ICommandHandler,
        IRequestHandler<CreateRolePermissionCommand, PubResponse>,
        IRequestHandler<UpdateRolePermissionCommand, PubResponse>,
        IRequestHandler<QueryRolePermissionCommand, PubResponse>,
        IRequestHandler<DeleteRolePermissionCommand, PubResponse>
    {
        private readonly IRolePermissionRepository _Repository;
        private readonly GlobalCore _globalCore;

        public RolePermissionCommandHandle(GlobalCore globalCore, IRolePermissionRepository Repository)
        {
            _globalCore = globalCore;
            _Repository = Repository;
        }

        public Task<PubResponse> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var destObject = request.Adapt<CreateRolePermissionCommand, RolePermission>();
            var flag = _Repository.Insert(destObject);
            return SucceedOrFail(flag);
        }

        public Task<PubResponse> Handle(UpdateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(QueryRolePermissionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}