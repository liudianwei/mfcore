using SqlSugar;

namespace MF.Job.Core.Business.Manager
{
    public class BaseManager
    {
        public SqlSugarClient db
        {
            get
            {
                return new DbManager().db;
            }
        }
    }
}