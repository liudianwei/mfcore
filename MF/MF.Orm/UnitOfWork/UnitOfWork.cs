using SqlSugar;
using System;
using System.Threading.Tasks;
using MF.Ioc;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MF.Orm.UnitOfWork
{
    [Component]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlSugarClient _client;
        private SqlSugarClient _Customerclient;
        private SqlSugarClient _CHclient; 
        private static IConfiguration _configuration;

        public UnitOfWork(SqlSugarClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
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
        /// <summary>
        /// ClickHouse
        /// </summary>
        /// <returns></returns>
        public SqlSugarClient GetCHDbClient()
        {
            var chConnectionString = _configuration["Orm:ChConnectionString"];
            bool.TryParse(_configuration["Log:SqlLog"], out bool flag);
            _CHclient = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.ClickHouse,
                ConnectionString = chConnectionString,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            }, db => {
                if (flag)
                {                   
                    //SQL执行完
                    db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        foreach (var item in pars)
                        {
                            sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                        }
                        sql = PretySql(sql);
                        Console.WriteLine($"");
                        Console.WriteLine($"ClickHouse--- 执行后SQL==>\r\n{sql}");
                        Console.WriteLine($"执行时间: {db.Ado.SqlExecutionTime.TotalSeconds}");
                    };
                }
            });
            return _CHclient;
        }

        private static string PretySql(string sql)
        {
            return sql.Replace("SELECT", "SELECT\r\n")
                       .Replace(",", ",\r\n")
                       .Replace("FROM", "\r\nFROM")
                       .Replace("WHERE", "\r\nWHERE")
                       .Replace("ORDER", "\r\nORDER")
                       .Replace("LIMIT", "\r\nLIMIT")
                       .Replace("Left JOIN", "\r\nLeft JOIN")
                       .Replace(")  AND  (", ")\r\nAND (")
                       .Replace(")   AND (", ")\r\nAND (");
        }
        /// <summary>
        /// 大数据写入 bulk插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool BulkCopy<T>(List<T> list) where T : BaseEntity, new()
        {
            return GetDbClient().Fastest<T>().BulkCopy(list) > 0;
        }

        /// <summary>
        /// 大数据更新 bulk插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool BulkUpdate<T>(List<T> list) where T : BaseEntity, new()
        {
            return GetDbClient().Fastest<T>().BulkUpdate(list) > 0;
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