using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using MF.NetCoreApp;
using MF.Orm.Repository;
using MF.Orm.UnitOfWork;

namespace DAL.UserCenter.Repository
{
    public class UserFavoriteRepository : BaseRepository<UserFavorite>, IUserFavoriteRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserFavoriteRepository(IUnitOfWork unitOfWork, GlobalCore globalCore) : base(unitOfWork, globalCore)
        {
            _unitOfWork = unitOfWork;
        }
    }
}