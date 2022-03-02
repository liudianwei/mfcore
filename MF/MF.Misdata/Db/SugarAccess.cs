using Common.Utils;

using Microsoft.Extensions.Logging;

using SqlSugar;

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.DBUtils
{
    public static partial class SugarAccess
    {
        public static readonly string deleteFlag = "2";
        private static readonly string newLine = Environment.NewLine;
        private static readonly string Tab = "\t";

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        /// <returns></returns>
        public static (SqlSugar.DbType, string) GetConnectionParam()
        {
            string type = ConfigHelper.GetAppseting("ConnectronStr:Use");
            return type switch
            {
                "MYSQL" => (SqlSugar.DbType.MySql, ConfigHelper.GetAppseting($"ConnectronStr:{type}")),
                "MSSQL" => (SqlSugar.DbType.SqlServer, ConfigHelper.GetAppseting($"ConnectronStr:{type}")),
                "SQLITE" => (SqlSugar.DbType.Sqlite, ConfigHelper.GetAppseting($"ConnectronStr:{type}")),
                "ORACLE" => (SqlSugar.DbType.Oracle, ConfigHelper.GetAppseting($"ConnectronStr:{type}")),
                "POSTGRESQL" => (SqlSugar.DbType.PostgreSQL, ConfigHelper.GetAppseting($"ConnectronStr:{type}")),
                _ => throw new Exception("不支持的数据库类型"),
            };
        }

        /// <summary>
        /// 获取数据库操作对象
        /// </summary>
        /// <param name="strConnectionstring"></param>
        /// <returns></returns>
        //public static SqlSugarClient Getdb()
        //{
        //    var db = ServiceResolve.ResolveS<SqlSugarClient>();
        //    //Console.WriteLine($"db-id: {db.ContextID}");
        //    return db;
        //    //var sqlSugarConfig = GetConnectionParam();
        //    //var config = new ConnectionConfig()
        //    //{
        //    //    DbType = sqlSugarConfig.Item1,
        //    //    ConnectionString = sqlSugarConfig.Item2,
        //    //    IsAutoCloseConnection = true,
        //    //    InitKeyType = InitKeyType.Attribute
        //    //};

        //    //var db = new SqlSugarClient(config);

        //    //var loggerFactory = new LoggerFactory();

        //    //ILogger log = null;
        //    //bool.TryParse(ConfigHelper.GetAppseting("ConnectronStr:SqlLog"), out bool flag);
        //    //if (flag)
        //    //{
        //    //    db.Ado.IsEnableLogEvent = true;
        //    //    db.Aop.OnLogExecuted = (sql, pars) =>
        //    //    {
        //    //        sql = Formatt(sql, pars);
        //    //        sql = PretySql(sql);
        //    //        Console.WriteLine(sql);
        //    //        SlowSql(db, log);
        //    //    };
        //    //    db.Aop.OnError = (exp) =>//执行SQL 错误事件
        //    //    {
        //    //        Console.WriteLine(exp.Sql);
        //    //    };
        //    //}
        //    //else
        //    //{
        //    //    db.Ado.IsEnableLogEvent = false;
        //    //}

        //    //return db;
        //}

        /// <summary>
        /// 获取数据库操作对象
        /// </summary>
        /// <param name="strConnectionstring"></param>
        /// <returns></returns>
        public static SqlSugarScope Getdb()
        {
            var db = ServiceResolve.ResolveS<SqlSugarScope>();
            //Console.WriteLine($"db-id: {db.ContextID}");
            return db;
        }

        #region 日志记录工具方法

        /// <summary>
        /// 数据库慢查询日志
        /// </summary>
        /// <param name="GetConn()"></param>
        /// <param name="log"></param>
        private static void SlowSql(SqlSugarClient db, ILogger log)
        {
            //执行时间超过1秒
            if (db.Ado.SqlExecutionTime.TotalSeconds > 1)
            {
                //代码CS文件名
                var fileName = db.Ado.SqlStackTrace.FirstFileName;
                //代码行数
                var fileLine = db.Ado.SqlStackTrace.FirstLine;
                //方法名
                var FirstMethodName = db.Ado.SqlStackTrace.FirstMethodName;
                //db.Ado.SqlStackTrace.MyStackTraceList[1].xxx 获取上层方法的信息

                Console.WriteLine($"{Tab}FileName: {fileName}{newLine}{Tab}FileLine: {fileLine}{newLine}{Tab}FirstMethodName: {FirstMethodName}");
            }
        }

        /// <summary>
        /// 参数格式化
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        private static string Formatt(string sql, SugarParameter[] pars)
        {
            Dictionary<int, List<SugarParameter>> sorts = new Dictionary<int, List<SugarParameter>>();
            int maxlen = 0;
            foreach (var item in pars)
            {
                if (maxlen < item.ParameterName.Length)
                {
                    maxlen = item.ParameterName.Length;
                }

                if (sorts.TryGetValue(item.ParameterName.Length, out var objlist))
                {
                    objlist.Add(item);
                }
                else
                {
                    sorts.Add(item.ParameterName.Length, new List<SugarParameter>() { item });
                }
            }

            while (maxlen > 0)
            {
                if (sorts.TryGetValue(maxlen, out var list))
                {
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                        }
                    }
                }
                maxlen--;
            }
            return sql;
        }

        /// <summary>
        /// 美化sql输出
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static string PretySql(string sql)
        {
            return sql.Replace("SELECT", $"SELECT{newLine}{Tab}")
                       .Replace(",", $",{newLine}{Tab}")
                       .Replace("FROM", $"{newLine}FROM")
                       .Replace("WHERE", $"{newLine}WHERE")
                       .Replace("ORDER", $"{newLine}ORDER")
                       .Replace("LIMIT", $"{newLine}LIMIT")
                       .Replace("Left JOIN", $"{newLine}{Tab}Left JOIN")
                       .Replace(")  AND  (", $"){newLine}  AND  (")
                       .Replace(")   AND (", $"){newLine}   AND (");
        }

        #endregion 日志记录工具方法

        /// <summary>
        /// 插入预处理
        /// </summary>
        /// <param name="entity">实体对象</param>
        private static void PreInsert(object entity)
        {
        }

        /// <summary>
        /// 更新预处理
        /// </summary>
        /// <param name="entity">实体对象</param>
        private static void PreUpdate(object entity)
        {
        }

        #region 新增

        /// <summary>
        /// 大数据写入 bulk插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool BulkCopy<T>(List<T> list) where T : BaseEntity, new()
        {
            return Getdb().Fastest<T>().BulkCopy(list) > 0;
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="IgnoreNullColumn">默认true 忽略空的列， false 不忽略空的列</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Insert<T>(T entity, bool IgnoreNullColumn = true) where T : BaseEntity, new()
        {
            PreInsert(entity);
            return Getdb().Insertable(entity).IgnoreColumns(IgnoreNullColumn).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录 自动忽略空的列
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool InsertIgnoreNullColumn<T>(T entity) where T : BaseEntity, new()
        {
            PreInsert(entity);
            return Getdb().Insertable(entity).IgnoreColumns(true).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录 忽略指定的列
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略的列数组</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool InsertIgnoreNullColumn<T>(T entity, params string[] ignoreColumns) where T : BaseEntity, new()
        {
            PreInsert(entity);
            return Getdb().Insertable(entity).IgnoreColumns(ignoreColumns).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录 指定使用数据库实例
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Insert<T>(SqlSugarClient _db, T entity) where T : BaseEntity, new()
        {
            PreInsert(entity);
            return _db.Insertable(entity).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>大数主键</returns>
        public static long InsertBigIdentity<T>(T entity) where T : BaseEntity, new()
        {
            PreInsert(entity);
            return Getdb().Insertable(entity).ExecuteReturnBigIdentity();
        }

        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Insert<T>(List<T> entitys) where T : BaseEntity, new()
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return false;
            }

            entitys = entitys.Select(i =>
            {
                PreInsert(i);
                return i;
            }).ToList();
            return Getdb().Insertable(entitys).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据实体 插入或更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>插入列表,更新列表</returns>
        public static (List<StorageableMessage<T>>, List<StorageableMessage<T>>) Insertable<T>(List<T> list) where T : BaseEntity, new()
        {
            var x = Getdb().Storageable(list).ToStorage();
            x.AsInsertable.ExecuteCommand();
            x.AsUpdateable.ExecuteCommand();

            var insertList = x.InsertList;
            var updateList = x.UpdateList;
            return (insertList, updateList);
        }

        /// <summary>
        /// 插入多条记录 返回id集合
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Insert<T>(List<T> entitys, out List<string> ids) where T : BaseEntity, new()
        {
            ids = new List<string>();
            if (entitys == null || entitys.Count <= 0)
            {
                return false;
            }

            entitys = entitys.Select(i =>
            {
                PreInsert(i);
                return i;
            }).ToList();
            var flag = Getdb().Insertable(entitys).ExecuteCommand() > 0;
            if (flag)
            {
                ids = entitys.Select(i => i.Id).ToList();
            }
            return flag;
        }

        /// <summary>
        /// 插入多条记录 自动忽略空的列
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool InsertIgnoreNullColumn<T>(List<T> entitys) where T : BaseEntity, new()
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return false;
            }
            entitys = entitys.Select(i =>
            {
                PreInsert(i);
                return i;
            }).ToList();
            return Getdb().Insertable(entitys).IgnoreColumns(true).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入多条记录 指定忽略的列集合
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <param name="ignoreColumns">忽略的列数组</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool InsertIgnoreNullColumn<T>(List<T> entitys, params string[] ignoreColumns) where T : BaseEntity, new()
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return false;
            }
            entitys = entitys.Select(i =>
            {
                PreInsert(i);
                return i;
            }).ToList();
            return Getdb().Insertable(entitys).IgnoreColumns(ignoreColumns).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 使用事务 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public static DbResult<bool> InsertTran<T>(T entity) where T : BaseEntity, new()
        {
            var result = Getdb().Ado.UseTran(() =>
            {
                PreInsert(entity);
                Getdb().Insertable(entity).ExecuteCommand();
            });
            return result;
        }

        /// <summary>
        /// 使用事务 插入多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static DbResult<bool> InsertTran<T>(List<T> entitys) where T : BaseEntity, new()
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return default;
            }
            var result = Getdb().Ado.UseTran(() =>
            {
                entitys = entitys.Select(i =>
                {
                    PreInsert(i);
                    return i;
                }).ToList();
                Getdb().Insertable(entitys).ExecuteCommand();
            });
            return result;
        }

        /// <summary>
        /// 插入一条记录 返回记录对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>刚插入的记录对象</returns>
        public static T InsertReturnEntity<T>(T entity) where T : BaseEntity, new()
        {
            PreInsert(entity);
            return Getdb().Insertable(entity).ExecuteReturnEntity();
        }

        /// <summary>
        /// 使用数据库锁参数 插入一条记录 返回记录对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="sqlWith">锁参数</param>
        /// <returns>刚插入的记录对象</returns>
        public static T InsertReturnEntityWithLock<T>(T entity, string sqlWith = SqlWith.UpdLock) where T : BaseEntity, new()
        {
            PreInsert(entity);
            return Getdb().Insertable(entity).With(sqlWith).ExecuteReturnEntity();
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool ExecuteCommand<T>(string sql, object parameters) where T : BaseEntity, new()
        {
            return Getdb().Ado.ExecuteCommand(sql, parameters) > 0;
        }

        /// <summary>
        /// 执行sql语句 指定参数列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool ExecuteCommand(string sql, params SugarParameter[] parameters)
        {
            return Getdb().Ado.ExecuteCommand(sql, parameters) > 0;
        }

        /// <summary>
        /// 执行sql语句 指定参数列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool ExecuteCommand(string sql, List<SugarParameter> parameters)
        {
            return Getdb().Ado.ExecuteCommand(sql, parameters) > 0;
        }

        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static (List<StorageableMessage<T>>, List<StorageableMessage<T>>) InsertOrUpdate<T>(List<T> list) where T : BaseEntity, new()
        {
            var x = Getdb().Storageable(list).ToStorage();
            x.AsInsertable.ExecuteCommand();
            x.AsUpdateable.ExecuteCommand();

            var insertList = x.InsertList;
            var updateList = x.UpdateList;
            return (insertList, updateList);
        }

        #endregion 新增

        #region 更新

        /// <summary>
        /// 大数据更新 bulk插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool BulkUpdate<T>(List<T> list) where T : BaseEntity, new()
        {
            return Getdb().Fastest<T>().BulkUpdate(list) > 0;
        }

        /// <summary>
        /// 乐观锁
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        private static IUpdateable<T> OptimisticLockUpdate<T>(T entity) where T : BaseEntity, new()
        {
            var oldVersion = entity.InnerVersion;
            entity.InnerVersion++;
            return Getdb().Updateable(entity).Where(o => o.Id.Equals(entity.Id)).Where(o => o.InnerVersion == oldVersion);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool UpdateEntity<T>(T entity) where T : BaseEntity, new()
        {
            PreUpdate(entity);
            return OptimisticLockUpdate(entity).ExecuteCommand() > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="where">where条件</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Update<T>(T entity, Expression<Func<T, bool>> where) where T : BaseEntity, new()
        {
            PreUpdate(entity);
            return OptimisticLockUpdate(entity).Where(where).ExecuteCommand() > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="where">where条件</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Update<T>(T entity, Expression<Func<T, object>> where) where T : BaseEntity, new()
        {
            PreUpdate(entity);
            return OptimisticLockUpdate(entity).UpdateColumns(where).ExecuteCommand() > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="updateColumns">更新的列</param>
        /// <param name="where">where条件</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Update<T>(T entity,
                              Expression<Func<T, object>> updateColumns,
                              Expression<Func<T, bool>> where) where T : BaseEntity, new()
        {
            PreUpdate(entity);
            return OptimisticLockUpdate(entity).UpdateColumns(updateColumns).Where(where).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="entity">实体对象</param>
        /// <param name="updateColumns">更新的列 表达式</param>
        /// <param name="where">where表达式</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Update<T>(SqlSugarClient _db,
                              T entity,
                              Expression<Func<T, object>> updateColumns,
                              Expression<Func<T, bool>> where) where T : BaseEntity, new()
        {
            PreUpdate(entity);
            return _db.Updateable(entity)
                .IsEnableUpdateVersionValidation()
                .SetColumns(it => it.InnerVersion == it.InnerVersion + 1)
                .UpdateColumns(updateColumns).Where(where).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新的实体</param>
        /// <param name="ignores">忽略更新的列列表</param>
        /// <param name="isNull">是否忽略更新 默认为true</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Update<T>(T entity, List<string> ignores = null, bool isNull = true) where T : BaseEntity, new()
        {
            if (ignores is null)
            {
                ignores = new List<string>()
                {
                    "create_by",
                    "create_time"
                };
            }
            PreUpdate(entity);
            return OptimisticLockUpdate(entity).IgnoreColumns(isNull).IgnoreColumns(ignores.ToArray()).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 带条件更新 多setColumns 多setColumnsIFs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="setColumns">更新列</param>
        /// <param name="setColumnsIFs">符合条件进行更新</param>
        /// <returns></returns>
        public static bool UpdateCustomer<T>(Expression<Func<T, bool>> where, List<Expression<Func<T, bool>>> setColumns, List<Ent<T>> setColumnsIFs = null) where T : BaseEntity, new()
        {
            var u = Getdb().Updateable<T>();

            if (setColumns != null)
            {
                foreach (var item in setColumns)
                {
                    u = u.SetColumns(item);
                }
            }

            if (setColumnsIFs != null)
            {
                foreach (var item in setColumnsIFs)
                {
                    u = u.SetColumnsIF(item.IsUpdateColumns, item.Columns);
                }
            }

            if (where != null)
            {
                u = u.Where(where);
            }

            return u.ExecuteCommand() > 0;
        }

        /// <summary>
        /// 带条件更新 单setColumns 单setColumnsIF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="setColumns"></param>
        /// <param name="setColumnsIFs"></param>
        /// <returns></returns>
        public static bool UpdateCustomer1<T>(Expression<Func<T, bool>> where, Expression<Func<T, bool>> setColumn, Ent<T> setColumnsIF = null) where T : BaseEntity, new()
        {
            return UpdateCustomer(where, new List<Expression<Func<T, bool>>> { setColumn }, setColumnsIF != null ? new List<Ent<T>>() { setColumnsIF } : null);
        }

        /// <summary>
        /// 带条件更新 多setColumns 单setColumnsIF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="setColumns"></param>
        /// <param name="setColumnsIFs"></param>
        /// <returns></returns>
        public static bool UpdateCustomer2<T>(Expression<Func<T, bool>> where, List<Expression<Func<T, bool>>> setColumns, Ent<T> setColumnsIF = null) where T : BaseEntity, new()
        {
            return UpdateCustomer(where, setColumns, setColumnsIF != null ? new List<Ent<T>> { setColumnsIF } : null);
        }

        /// <summary>
        /// 带条件更新 单setColumns 多setColumnsIF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="setColumns"></param>
        /// <param name="setColumnsIFs"></param>
        /// <returns></returns>
        public static bool UpdateCustomer3<T>(Expression<Func<T, bool>> where, Expression<Func<T, bool>> setColumn, List<Ent<T>> setColumnsIFs = null) where T : BaseEntity, new()
        {
            return UpdateCustomer(where, new List<Expression<Func<T, bool>>> { setColumn }, setColumnsIFs);
        }

        public class Ent<T>
        {
            public bool IsUpdateColumns { get; set; }
            public Expression<Func<T, bool>> Columns { get; set; }
        }

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Update<T>(List<T> entitys) where T : BaseEntity, new()
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return false;
            }

            entitys.ForEach(i =>
            {
                PreUpdate(i);
            });
            return Getdb().Updateable(entitys).IsEnableUpdateVersionValidation().ExecuteCommand() > 0;
        }

        #endregion 更新

        #region 事务

        /// <summary>
        ///
        /// </summary>
        /// <param name="action">事务内容</param>
        /// <returns>true 成功，false 失败</returns>
        public static DbResult<bool> UseTran(Action action)
        {
            var result = Getdb().Ado.UseTran(() => action());
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="action">事务内容</param>
        /// <returns>true 成功，false 失败</returns>
        public static DbResult<bool> UseTran(SqlSugarClient client, Action action)
        {
            var result = client.Ado.UseTran(() => action());
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <returns>true 成功，false 失败</returns>
        public static bool UseTran2(Action action)
        {
            var result = Getdb().Ado.UseTran(() => action());
            return result.IsSuccess;
        }

        #endregion 事务

        #region 删除

        /// <summary>
        /// 删除记录 默认逻辑删除
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Delete<T>(Expression<Func<T, bool>> where, bool logic = true) where T : BaseEntity, new()
        {
            if (logic)
            {
                return Getdb().Updateable<T>().Where(where).SetColumns(it => it.State == deleteFlag).ExecuteCommand() > 0;
            }
            return Getdb().Deleteable<T>().Where(where).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据主键列表删除记录 默认逻辑删除
        /// </summary>
        /// <param name="primaryKeys">主键数组</param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Delete<T>(string[] primaryKeys, bool logic = true) where T : BaseEntity, new()
        {
            if (logic)
            {
                var ids = primaryKeys.ToList();
                return Getdb().Updateable<T>().Where(i => ids.Contains(i.Id)).SetColumns(it => it.State == deleteFlag).ExecuteCommand() > 0;
            }
            return Getdb().Deleteable<T>().In(primaryKeys).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据对象删除记录 默认逻辑删除
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool Delete<T>(T obj, bool logic = true) where T : BaseEntity, new()
        {
            if (logic)
            {
                obj.State = deleteFlag;
                return Getdb().Updateable<T>(obj).ExecuteCommand() > 0;
            }
            return Getdb().Deleteable<T>().In(obj).ExecuteCommand() > 0;
        }

        #endregion 删除

        #region 查询

        /// <summary>
        /// 查询 默认逻辑查询
        /// </summary>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>查询对象</returns>
        public static ISugarQueryable<T> Queryable<T>(bool logic = true) where T : BaseEntity, new()
        {
            return Getdb().Queryable<T>().WhereIF(logic, i => i.State != deleteFlag);
        }

        /// <summary>
        /// 按主键查询 默认逻辑查询
        /// </summary>
        /// <param name="id"》主键</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>查询对象</returns>
        public static T QueryByPK<T>(string id, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).InSingle(id);
        }

        /// <summary>
        /// 按主键查询 默认逻辑查询
        /// </summary>
        /// <param name="objIDs">主键列表</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>查询对象</returns>
        public static List<T> QueryByPKs<T, KTYPE>(KTYPE objIDs, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).In(objIDs).ToList();
        }

        /// <summary>
        /// 判断记录是否存在 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>true 成功，false 失败</returns>
        public static bool IsAny<T>(Expression<Func<T, bool>> where, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).Where(where).Any();
        }

        /// <summary>
        /// 根据表名查询记录 物理查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="shortName">别名</param>
        /// <returns>动态的查询对象</returns>
        public static ISugarQueryable<ExpandoObject> Queryable(string tableName, string shortName)
        {
            return Getdb().Queryable(tableName, shortName);
        }

        /// <summary>
        /// 逻辑理查询 默认逻辑查询 查询所有数据
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public static List<T> QueryAll<T>(bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 异步逻辑理查询 默认逻辑查询 查询所有数据
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public static Task<List<T>> QueryableToListAsync<T>(bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).OrderBy(i => i.CreateTime, OrderByType.Desc).ToListAsync();
        }

        /// <summary>
        /// 查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public static List<T> QueryableToList<T>(Expression<Func<T, bool>> where, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderBy"></param>
        /// <param name="ordinal"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public static List<T> QueryableToList<T>(Expression<Func<T, bool>> where,
                                          Expression<Func<T, object>> orderBy,
                                          OrderByType ordinal = OrderByType.Asc,
                                          bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).Where(where).OrderBy(orderBy, ordinal).ToList();
        }

        /// <summary>
        /// 异步查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public static Task<List<T>> QueryableToListAsync<T>(Expression<Func<T, bool>> where, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToListAsync();
        }

        /// <summary>
        /// 查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        public static T QueryableToEntity<T>(Expression<Func<T, bool>> where, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).Where(where).First();
        }

        /// <summary>
        /// 异步查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        public static Task<T> QueryableToEntityAsync<T>(Expression<Func<T, bool>> where, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).Where(where).FirstAsync();
        }

        /// <summary>
        /// 查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderBy"></param>
        /// <param name="ordinal"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        public static T QueryableToEntity<T>(Expression<Func<T, bool>> where,
                                      Expression<Func<T, object>> orderBy,
                                      OrderByType ordinal = OrderByType.Desc,
                                      bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(logic).Where(where).OrderBy(orderBy, ordinal).First();
        }

        /// <summary>
        /// 逻辑理查询 默认逻辑查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns></returns>
        private static ISugarQueryable<T> Queryable<T>(string tableName, bool logic = true) where T : BaseEntity, new()
        {
            return Getdb().Queryable<T>(tableName).WhereIF(logic, i => i.State != deleteFlag);
        }

        /// <summary>
        /// 根据指定表名称查询记录列表 默认逻辑查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public static List<T> QueryableToList<T>(string tableName, bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(tableName, logic).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 根据指定表名称 和 where条件 查询记录列表 默认逻辑查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">where表达式</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public static List<T> QueryableToList<T>(string tableName,
                                          Expression<Func<T, bool>> where,
                                          bool logic = true) where T : BaseEntity, new()
        {
            return Queryable<T>(tableName, logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 分页查询 返回记录列表和记录总数 默认逻辑查询
        /// </summary>
        /// <param name="where">where表达式</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表 和 记录总数</returns>
        public static (List<T>, int) QueryableToPage<T>(Expression<Func<T, bool>> where,
                                                 int pageIndex = 0,
                                                 int pageSize = 10,
                                                 bool logic = true) where T : BaseEntity, new()
        {
            int totalNumber = 0;
            var list = Queryable<T>(logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalNumber);
            return (list, totalNumber);
        }

        /// <summary>
        /// 根据排序条件 分页查询 返回记录列表和记录总数 默认逻辑查询
        /// </summary>
        /// <param name="where">where表达式</param>
        /// <param name="orderBy">排序列</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表 和 记录总数</returns>
        public static (List<T>, int) QueryableToPage<T>(Expression<Func<T, bool>> where,
                                                 string orderBy,
                                                 int pageIndex = 0,
                                                 int pageSize = 10,
                                                 bool logic = true) where T : BaseEntity, new()
        {
            int totalNumber = 0;
            var list = Queryable<T>(logic).Where(where).OrderBy(orderBy).ToPageList(pageIndex, pageSize, ref totalNumber);
            return (list, totalNumber);
        }

        /// <summary>
        /// 分页查询 返回记录列表和记录总数 默认逻辑查询
        /// </summary>
        /// <param name="where">where表达式</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="ordinal">排序序数 DESC ASC</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表 和 记录总数</returns>
        public static (List<T>, int) QueryableToPage<T>(Expression<Func<T, bool>> where,
                                                 Expression<Func<T, object>> orderBy,
                                                 string ordinal,
                                                 int pageIndex = 0,
                                                 int pageSize = 10,
                                                 bool logic = true) where T : BaseEntity, new()
        {
            int totalNumber = 0;

            if (ordinal.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            {
                var list = Queryable<T>(logic).Where(where).OrderBy(orderBy, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalNumber);
                return (list, totalNumber);
            }
            else
            {
                var list = Queryable<T>(logic).Where(where).OrderBy(orderBy, OrderByType.Asc).ToPageList(pageIndex, pageSize, ref totalNumber);
                return (list, totalNumber);
            }
        }

        /// <summary>
        /// sql原生查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>记录列表</returns>
        public static List<T> SqlQueryToList<T>(string sql, object parameters = null) where T : BaseEntity, new()
        {
            return Getdb().Ado.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// sql原生查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>记录列表</returns>
        public static DataTable SqlGetDataTable(string sql, object parameters = null)
        {
            return Getdb().Ado.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// sql原生查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>记录列表</returns>
        public static bool SqlExecuteCommand(string sql, object parameters = null)
        {
            return Getdb().Ado.ExecuteCommand(sql, parameters) > 0;
        }

        #endregion 查询

        #region 存储过程

        /// <summary>
        /// 调用存储过程 返回bool
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public static bool UseStoredProcedure(string procedureName, List<SugarParameter> parameters = null)
        {
            return Getdb().Ado.UseStoredProcedure().ExecuteCommand(procedureName, parameters) > 0;
        }

        /// <summary>
        /// 调用存储过程 返回bool
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public static bool UseStoredProcedure(string procedureName, SugarParameter[] parameters)
        {
            return Getdb().Ado.UseStoredProcedure().ExecuteCommand(procedureName, parameters) > 0;
        }

        /// <summary>
        /// 调用存储过程 返回bool
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public static bool UseStoredProcedureToBool(string procedureName, List<SugarParameter> parameters = null)
        {
            return Getdb().Ado.UseStoredProcedure().GetDataTable(procedureName, parameters) != null;
        }

        /// <summary>
        /// 调用存储过程 返回bool
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public static bool UseStoredProcedureToBool(string procedureName, SugarParameter[] parameters)
        {
            return Getdb().Ado.UseStoredProcedure().GetDataTable(procedureName, parameters) != null;
        }

        /// <summary>
        /// 调用存储过程 不带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public static DataTable UseStoredProcedureToDataTable(string procedureName, List<SugarParameter> parameters = null)
        {
            return Getdb().Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
        }

        /// <summary>
        /// 调用存储过程 不带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public static DataTable UseStoredProcedureToDataTable(string procedureName, SugarParameter[] parameters)
        {
            return Getdb().Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
        }

        /// <summary>
        /// 调用存储过程 带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue, true));  output
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集 和 返回参数</returns>
        public static (DataTable, List<SugarParameter>) UseStoredProcedureToTuple(string procedureName, List<SugarParameter> parameters)
        {
            var dt = Getdb().Ado.UseStoredProcedure().GetDataSetAll(procedureName, parameters);
            return (dt.Tables[0], parameters);
        }

        /// <summary>
        /// 调用存储过程 带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue, true));  output
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集 和 返回参数</returns>
        public static (DataTable, SugarParameter[]) UseStoredProcedureToTuple(string procedureName, SugarParameter[] parameters)
        {
            var dt = Getdb().Ado.UseStoredProcedure().GetDataSetAll(procedureName, parameters);
            return (dt.Tables[0], parameters);
        }

        #endregion 存储过程

        #region 其它

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        public static DateTime GetDbTime()
        {
            return Getdb().GetDate();
        }

        /// <summary>
        /// 清除表缓存
        /// </summary>
        public static void RemoveCache<T>()
        {
            var cacheService = Getdb().CurrentConnectionConfig.ConfigureExternalServices.DataInfoCacheService;
            string tableName = Getdb().EntityMaintenance.GetTableName<T>();
            var keys = cacheService.GetAllKey<string>();
            if (keys != null && keys.Count() > 0)
            {
                foreach (var item in keys)
                {
                    if (item.ToLower().Contains($".{tableName.ToLower()}."))
                    {
                        cacheService.Remove<string>(item);
                    }
                }
            }
        }

        #endregion 其它
    }
}