using System;
using System.Threading;
using System.Threading.Tasks;

using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using Mapster;

using MediatR;

using MF.FluentValidation;
using MF.NetCoreApp;
using MF.Orm;

using UserCenter.Commands;

namespace UserCenter.CommandHandles
{
    public class UserFavoriteCommandHandle : ICommandHandler,
        IRequestHandler<CreateUserFavoriteCommand, PubResponse>,
        IRequestHandler<UpdateUcUserFavoriteCommand, PubResponse>,
        IRequestHandler<QueryUserFavoriteCommand, PubResponse>,
        IRequestHandler<DeleteUcUserFavoriteCommand, PubResponse>
    {
        private readonly IUserFavoriteRepository _Repository;
        private readonly GlobalCore _globalCore;

        public UserFavoriteCommandHandle(GlobalCore globalCore, IUserFavoriteRepository Repository)
        {
            _globalCore = globalCore;
            _Repository = Repository;
        }

        public Task<PubResponse> Handle(CreateUserFavoriteCommand request, CancellationToken cancellationToken)
        {
            var destObject = request.Adapt<CreateUserFavoriteCommand, UserFavorite>();
            var flag = _Repository.Insert(destObject);
            return SucceedOrFail(flag);
        }

        public Task<PubResponse> Handle(UpdateUcUserFavoriteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(QueryUserFavoriteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PubResponse> Handle(DeleteUcUserFavoriteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}