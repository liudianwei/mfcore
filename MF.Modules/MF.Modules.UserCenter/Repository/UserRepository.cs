using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using MF.NetCoreApp;
using MF.Orm.Repository;
using MF.Orm.UnitOfWork;

namespace DAL.UserCenter.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalCore _globalCore;

        public UserRepository(IUnitOfWork unitOfWork, GlobalCore globalCore) : base(unitOfWork, globalCore)
        {
            _unitOfWork = unitOfWork;
            _globalCore = globalCore;
        }
    }
}