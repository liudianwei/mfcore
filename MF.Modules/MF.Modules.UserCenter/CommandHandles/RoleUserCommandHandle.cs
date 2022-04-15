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
    public class RoleUserCommandHandle : ICommandHandler,
        IRequestHandler<CreateRoleUserCommand, PubResponse>,
        IRequestHandler<UpdateRoleUserCommand, PubResponse>,
        IRequestHandler<QueryRoleUserCommand, PubResponse>,
        IRequestHandler<DeleteRoleUserCommand, PubResponse>
    {
        private readonly IRoleUserRepository _Repository;
        private readonly GlobalCore _globalCore;

        public RoleUserCommandHandle(GlobalCore globalCore, IRoleUserRepository Repository)
        {
            _globalCore = globalCore;
            _Repository = Repository;
        }

        public Task<PubResponse> Handle(CreateRoleUserCommand request, CancellationToken cancellationToken)
        {
            var destObject = request.Adapt<CreateRoleUserCommand, RoleUser>();
            var flag = _Repository.Insert(destObject);
            return SucceedOrFail(flag);
        }

        public Task<PubResponse> Handle(UpdateRoleUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(QueryRoleUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(DeleteRoleUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}