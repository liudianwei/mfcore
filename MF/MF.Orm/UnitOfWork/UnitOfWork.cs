using SqlSugar;
using System;
using System.Threading.Tasks;
using MF.Ioc;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace MF.Orm.UnitOfWork
{
    [Component]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlSugarClient _client;
        private SqlSugarClient _Customerclient;

        public UnitOfWork(SqlSugarClient client)
        {
            _client = client;
        }

        public SqlSugarClient GetDbClient()
        {
            return _client as SqlSugarClient;
        }

        public SqlSugarClient GetCustomerDbClient(string connstr, DbType type)
        {
            _Customerclient = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = type,
                ConnectionString = connstr,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            return _Customerclient;
        }

        public DbResult<bool> UseTran(Action action, Action<Exception> ex = null)
        {
            return GetDbClient().Ado.UseTran(action, ex);
        }

        public DbResult<bool> UseCustomerTran(Action action, Action<Exception> ex = null)
        {
            if (_Customerclient == null)
            {
                return UseTran(action, ex);
            }
            return _Customerclient.Ado.UseTran(action, ex);
        }

        public Task<DbResult<bool>> UseTranAsync(Func<Task> action, Action<Exception> ex = null)
        {
            return GetDbClient().Ado.UseTranAsync(action, ex);
        }

        public Task<DbResult<bool>> UseCustomerTranAsync(Func<Task> action, Action<Exception> ex = null)
        {
            if (_Customerclient == null)
            {
                return UseTranAsync(action, ex);
            }
            return _Customerclient.Ado.UseTranAsync(action, ex);
        }

        /// <summary>
        /// 对象转SugarParameters的数组
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SugarParameter> GetSugarParameters<T>(T Info) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            SugarParameter[] arParms = new SugarParameter[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                var direct = properties[i].GetCustomAttribute<StoreParamDirect>();
                var name = (direct.Name == "" ? properties[i].Name : direct.Name);
                var isOutPut = (direct?.Direction == System.Data.ParameterDirection.Output || direct?.Direction == System.Data.ParameterDirection.InputOutput);
                arParms[i] = new SugarParameter($"@{name}", properties[i].GetValue(Info), isOutPut);
            }
            return arParms.ToList();
        }
    }

    public sealed class StoreParamDirect : Attribute
    {
        public StoreParamDirect()
        {
            Name = "";
            Direction = System.Data.ParameterDirection.Input;
        }

        public StoreParamDirect(string name)
        {
            Name = name;
            Direction = System.Data.ParameterDirection.Input;
        }

        public StoreParamDirect(System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            Name = "";
            Direction = direction;
        }

        public StoreParamDirect(string name = "", System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            Name = name;
            Direction = direction;
        }

        public string Name { get; set; }

        public System.Data.ParameterDirection Direction { get; set; }
    }
}