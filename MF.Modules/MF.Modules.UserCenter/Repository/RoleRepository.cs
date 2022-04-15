using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using MF.NetCoreApp;
using MF.Orm.Repository;
using MF.Orm.UnitOfWork;

namespace DAL.UserCenter.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleRepository(IUnitOfWork unitOfWork, GlobalCore globalCore) : base(unitOfWork, globalCore)
        {
            _unitOfWork = unitOfWork;
        }
    }
}