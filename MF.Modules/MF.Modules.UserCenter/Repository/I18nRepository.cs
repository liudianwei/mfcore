using DAL.MDCenter.Entities;
using DAL.MDCenter.IRepository;

using MF.NetCoreApp;
using MF.Orm.Repository;
using MF.Orm.UnitOfWork;

namespace DAL.MDCenter.Repository
{
    public class I18nRepository : BaseRepository<I18n>, II18nRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public I18nRepository(IUnitOfWork unitOfWork, GlobalCore globalCore) : base(unitOfWork, globalCore)
        {
            _unitOfWork = unitOfWork;
        }
    }
}