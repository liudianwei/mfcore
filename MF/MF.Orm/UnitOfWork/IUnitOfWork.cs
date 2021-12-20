using SqlSugar;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MF.Orm.UnitOfWork
{
    public interface IUnitOfWork
    {
        SqlSugarClient GetDbClient();

        SqlSugarClient GetCustomerDbClient(string connstr, DbType type);

        DbResult<bool> UseTran(Action action, Action<Exception> ex = null);

        DbResult<bool> UseCustomerTran(Action action, Action<Exception> ex = null);

        Task<DbResult<bool>> UseTranAsync(Func<Task> action, Action<Exception> ex = null);

        Task<DbResult<bool>> UseCustomerTranAsync(Func<Task> action, Action<Exception> ex = null);

        List<SugarParameter> GetSugarParameters<T>(T Info) where T : class;
    }
}