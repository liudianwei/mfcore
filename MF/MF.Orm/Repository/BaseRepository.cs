using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using SqlSugar;

using MF.NetCoreApp;
using MF.Orm.UnitOfWork;

namespace MF.Orm.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        public SqlSugarClient db;
        private readonly string deleteFlag = "2";
        private readonly GlobalCore globalCore;

        public BaseRepository(IUnitOfWork unitOfWork, GlobalCore _globalCore)
        {
            db = unitOfWork.GetDbClient() ?? throw new ArgumentNullException(nameof(unitOfWork));
            globalCore = _globalCore;
        }

        /// <summary>
        /// 插入预处理
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void PreInsert(T entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.InnerVersion = 0;
            entity.State ??= "0";

            entity.CreateTime = DateTime.Now;
            if (entity.Creator == null || entity.Creator.Trim() == "")
            {
                entity.Creator = globalCore.UserConcatName;
            }

            entity.UpdateTime = DateTime.Now;
            if (entity.Updator == null || entity.Updator.Trim() == "")
            {
                entity.Updator = globalCore.UserConcatName;
            }
        }

        /// <summary>
        /// 更新预处理
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void PreUpdate(T entity)
        {
            entity.UpdateTime = DateTime.Now;
            //if (entity.Updator == null || entity.Updator.Trim() == "")
            //{
            entity.Updator = globalCore.UserConcatName;
            //}
        }

        #region 新增

        /// <summary>
        /// 大数据写入 bulk插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool BulkCopy(List<T> list)
        {
            return db.Fastest<T>().BulkCopy(list) > 0;
        }
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="IgnoreNullColumn">默认true 忽略空的列， false 不忽略空的列</param>
        /// <returns>true 成功，false 失败</returns>
        public bool Insert(T entity, bool IgnoreNullColumn = true)
        {
            PreInsert(entity);
            return db.Insertable(entity).IgnoreColumns(IgnoreNullColumn).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录 自动忽略空的列
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public bool InsertIgnoreNullColumn(T entity)
        {
            PreInsert(entity);
            return db.Insertable(entity).IgnoreColumns(true).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录 忽略指定的列
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略的列数组</param>
        /// <returns>true 成功，false 失败</returns>
        public bool InsertIgnoreNullColumn(T entity, params string[] ignoreColumns)
        {
            PreInsert(entity);
            return db.Insertable(entity).IgnoreColumns(ignoreColumns).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录 指定使用数据库实例
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public bool Insert(SqlSugarClient _db, T entity)
        {
            PreInsert(entity);
            return _db.Insertable(entity).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>大数主键</returns>
        public long InsertBigIdentity(T entity)
        {
            PreInsert(entity);
            return db.Insertable(entity).ExecuteReturnBigIdentity();
        }

        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public bool Insert(List<T> entitys)
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
            return db.Insertable(entitys).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入多条记录 返回id集合
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public bool Insert(List<T> entitys, out List<string> ids)
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
            var flag = db.Insertable(entitys).ExecuteCommand() > 0;
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
        public bool InsertIgnoreNullColumn(List<T> entitys)
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
            return db.Insertable(entitys).IgnoreColumns(true).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入多条记录 指定忽略的列集合
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <param name="ignoreColumns">忽略的列数组</param>
        /// <returns>true 成功，false 失败</returns>
        public bool InsertIgnoreNullColumn(List<T> entitys, params string[] ignoreColumns)
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
            return db.Insertable(entitys).IgnoreColumns(ignoreColumns).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 使用事务 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public DbResult<bool> InsertTran(T entity)
        {
            var result = db.Ado.UseTran(() =>
            {
                PreInsert(entity);
                db.Insertable(entity).ExecuteCommand();
            });
            return result;
        }

        /// <summary>
        /// 使用事务 插入多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public DbResult<bool> InsertTran(List<T> entitys)
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return default;
            }
            var result = db.Ado.UseTran(() =>
            {
                entitys = entitys.Select(i =>
                {
                    PreInsert(i);
                    return i;
                }).ToList();
                db.Insertable(entitys).ExecuteCommand();
            });
            return result;
        }

        /// <summary>
        /// 插入一条记录 返回记录对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>刚插入的记录对象</returns>
        public T InsertReturnEntity(T entity)
        {
            PreInsert(entity);
            return db.Insertable(entity).ExecuteReturnEntity();
        }

        /// <summary>
        /// 使用数据库锁参数 插入一条记录 返回记录对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="sqlWith">锁参数</param>
        /// <returns>刚插入的记录对象</returns>
        public T InsertReturnEntityWithLock(T entity, string sqlWith = SqlWith.UpdLock)
        {
            PreInsert(entity);
            return db.Insertable(entity).With(sqlWith).ExecuteReturnEntity();
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        public bool ExecuteCommand(string sql, object parameters)
        {
            return db.Ado.ExecuteCommand(sql, parameters) > 0;
        }

        /// <summary>
        /// 执行sql语句 指定参数列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        public bool ExecuteCommand(string sql, params SugarParameter[] parameters)
        {
            return db.Ado.ExecuteCommand(sql, parameters) > 0;
        }

        /// <summary>
        /// 执行sql语句 指定参数列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        public bool ExecuteCommand(string sql, List<SugarParameter> parameters)
        {
            return db.Ado.ExecuteCommand(sql, parameters) > 0;
        }

        #endregion 新增

        #region 更新

        /// <summary>
        /// 大数据更新 bulk插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool BulkUpdate(List<T> list)
        {
            return db.Fastest<T>().BulkUpdate(list) > 0;
        }

        /// <summary>
        /// 乐观锁
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        private IUpdateable<T> OptimisticLockUpdate(T entity)
        {
            var oldVersion = entity.InnerVersion;
            entity.InnerVersion++;// 版本号加1
            //return db.Updateable(entity).Where(i => i.TenantCode.Equals(globalCore.TenantCode))
            //    .Where(o => o.Id.Equals(entity.Id)).Where(o => o.InnerVersion == oldVersion);
            return db.Updateable(entity).Where(o => o.Id.Equals(entity.Id)).Where(o => o.InnerVersion == oldVersion);

            //return db.Updateable(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        public bool UpdateEntity(T entity)
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
        public bool Update(T entity, Expression<Func<T, bool>> where)
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
        public bool Update(T entity, Expression<Func<T, object>> where)
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
        public bool Update(T entity, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> where)
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
        public bool Update(SqlSugarClient _db, T entity, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> where)
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
        public bool Update(T entity, List<string> ignores = null, bool isNull = true)
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
        /// 更新多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        public bool Update(List<T> entitys)
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return false;
            }

            entitys.ForEach(i =>
                    {
                        PreUpdate(i);
                    });
            //ts = ts.Select(i => { i.UpdateTime ??= DateTime.Now; return i; }).ToList();
            return db.Updateable(entitys).IsEnableUpdateVersionValidation().ExecuteCommand() > 0;
        }

        #endregion 更新

        #region 事务

        /// <summary>
        ///
        /// </summary>
        /// <param name="action">事务内容</param>
        /// <returns>true 成功，false 失败</returns>
        public DbResult<bool> UseTran(Action action)
        {
            var result = db.Ado.UseTran(() => action());
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="action">事务内容</param>
        /// <returns>true 成功，false 失败</returns>
        public DbResult<bool> UseTran(SqlSugarClient client, Action action)
        {
            var result = client.Ado.UseTran(() => action());
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <returns>true 成功，false 失败</returns>
        public bool UseTran2(Action action)
        {
            var result = db.Ado.UseTran(() => action());
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
        public bool Delete(Expression<Func<T, bool>> where, bool logic = true)
        {
            if (logic)
            {
                return db.Updateable<T>().Where(where).SetColumns(it => it.State == deleteFlag).ExecuteCommand() > 0;
            }
            return db.Deleteable<T>().Where(where).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据主键列表删除记录 默认逻辑删除
        /// </summary>
        /// <param name="primaryKeys">主键数组</param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        public bool Delete(string[] primaryKeys, bool logic = true)
        {
            if (logic)
            {
                var ids = primaryKeys.ToList();
                return db.Updateable<T>().Where(i => ids.Contains(i.Id)).SetColumns(it => it.State == deleteFlag).ExecuteCommand() > 0;
            }
            return db.Deleteable<T>().In(primaryKeys).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据对象删除记录 默认逻辑删除
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        public bool Delete(T obj, bool logic = true)
        {
            if (logic)
            {
                obj.State = deleteFlag;
                return db.Updateable<T>(obj).ExecuteCommand() > 0;
                //return db.Updateable<T>(obj).Where(i=>1==1).SetColumns(it => it.State == deleteFlag).ExecuteCommand() > 0;
            }
            return db.Deleteable<T>().In(obj).ExecuteCommand() > 0;
        }

        #endregion 删除

        #region 查询

        /// <summary>
        /// 查询 默认逻辑查询
        /// </summary>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>查询对象</returns>
        public ISugarQueryable<T> Queryable(bool logic = true)
        {
            //string tenantCode = globalCore.TenantCode ?? _tenantCode;
            //return db.Queryable<T>().WhereIF(tenantCode.Length > 0, i => i.TenantCode.Equals(tenantCode)).WhereIF(logic, i => i.State != deleteFlag);
            return db.Queryable<T>().WhereIF(logic, i => i.State != deleteFlag);
        }

        /// <summary>
        /// 按主键查询 默认逻辑查询
        /// </summary>
        /// <param name="id"》主键</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>查询对象</returns>
        public T QueryByPK(string id, bool logic = true)
        {
            return Queryable(logic).InSingle(id);
        }

        /// <summary>
        /// 按主键查询 默认逻辑查询
        /// </summary>
        /// <param name="objIDs">主键列表</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>查询对象</returns>
        public List<T> QueryByPKs<KTYPE>(KTYPE objIDs, bool logic = true)
        {
            return Queryable(logic).In(objIDs).ToList();
        }

        /// <summary>
        /// 判断记录是否存在 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>true 成功，false 失败</returns>
        public bool IsAny(Expression<Func<T, bool>> where, bool logic = true)
        {
            return Queryable(logic).Where(where).Any();
        }

        /// <summary>
        /// 根据表名查询记录 物理查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="shortName">别名</param>
        /// <returns>动态的查询对象</returns>
        public ISugarQueryable<ExpandoObject> Queryable(string tableName, string shortName)
        {
            return db.Queryable(tableName, shortName);
        }

        /// <summary>
        /// 逻辑理查询 默认逻辑查询 查询所有数据
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public List<T> QueryAll(bool logic = true)
        {
            return Queryable(logic).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 异步逻辑理查询 默认逻辑查询 查询所有数据
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public Task<List<T>> QueryableToListAsync(bool logic = true)
        {
            return Queryable(logic).OrderBy(i => i.CreateTime, OrderByType.Desc).ToListAsync();
        }

        /// <summary>
        /// 查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public List<T> QueryableToList(Expression<Func<T, bool>> where, bool logic = true)
        {
            return Queryable(logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderBy"></param>
        /// <param name="ordinal"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public List<T> QueryableToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, OrderByType ordinal = OrderByType.Asc, bool logic = true)
        {
            return Queryable(logic).Where(where).OrderBy(orderBy, ordinal).ToList();
        }

        /// <summary>
        /// 异步查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public Task<List<T>> QueryableToListAsync(Expression<Func<T, bool>> where, bool logic = true)
        {
            return Queryable(logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToListAsync();
        }

        /// <summary>
        /// 查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        public T QueryableToEntity(Expression<Func<T, bool>> where, bool logic = true)
        {
            return Queryable(logic).Where(where).First();
        }

        /// <summary>
        /// 异步查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        public Task<T> QueryableToEntityAsync(Expression<Func<T, bool>> where, bool logic = true)
        {
            return Queryable(logic).Where(where).FirstAsync();
        }

        /// <summary>
        /// 查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderBy"></param>
        /// <param name="ordinal"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        public T QueryableToEntity(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, OrderByType ordinal = OrderByType.Desc, bool logic = true)
        {
            return Queryable(logic).Where(where).OrderBy(orderBy, ordinal).First();
        }

        /// <summary>
        /// 逻辑理查询 默认逻辑查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns></returns>
        private ISugarQueryable<T> Queryable(string tableName, bool logic = true)
        {
            return db.Queryable<T>(tableName).WhereIF(logic, i => i.State != deleteFlag);
        }

        /// <summary>
        /// 根据指定表名称查询记录列表 默认逻辑查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public List<T> QueryableToList(string tableName, bool logic = true)
        {
            return Queryable(tableName, logic).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 根据指定表名称 和 where条件 查询记录列表 默认逻辑查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">where表达式</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        public List<T> QueryableToList(string tableName, Expression<Func<T, bool>> where, bool logic = true)
        {
            return Queryable(tableName, logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 分页查询 返回记录列表和记录总数 默认逻辑查询
        /// </summary>
        /// <param name="where">where表达式</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表 和 记录总数</returns>
        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> where, int pageIndex = 0, int pageSize = 10, bool logic = true)
        {
            int totalNumber = 0;
            var list = Queryable(logic).Where(where).OrderBy(i => i.CreateTime, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalNumber);
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
        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> where, string orderBy, int pageIndex = 0, int pageSize = 10, bool logic = true)
        {
            int totalNumber = 0;
            var list = Queryable(logic).Where(where).OrderBy(orderBy).ToPageList(pageIndex, pageSize, ref totalNumber);
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
        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, string ordinal, int pageIndex = 0, int pageSize = 10, bool logic = true)
        {
            int totalNumber = 0;

            if (ordinal.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            {
                var list = Queryable(logic).Where(where).OrderBy(orderBy, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalNumber);
                return (list, totalNumber);
            }
            else
            {
                var list = Queryable(logic).Where(where).OrderBy(orderBy, OrderByType.Asc).ToPageList(pageIndex, pageSize, ref totalNumber);
                return (list, totalNumber);
            }
        }

        /// <summary>
        /// sql原生查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>记录列表</returns>
        public List<T> SqlQueryToList(string sql, object parameters = null)
        {
            return db.Ado.SqlQuery<T>(sql, parameters);
        }

        #endregion 查询

        #region 存储过程

        /// <summary>
        /// 调用存储过程 不带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public DataTable UseStoredProcedureToDataTable(string procedureName, List<SugarParameter> parameters)
        {
            return db.Ado.UseStoredProcedure().GetDataTable(procedureName, parameters);
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
        public (DataTable, List<SugarParameter>) UseStoredProcedureToTuple(string procedureName, List<SugarParameter> parameters)
        {
            //var result = (db.Ado.UseStoredProcedure().GetDataTable(procedureName, parameters), parameters);
            var dt = db.Ado.UseStoredProcedure().GetDataSetAll(procedureName, parameters);
            return (dt.Tables[0], parameters);
        }

        #endregion 存储过程

        #region 其它

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        public DateTime GetDbTime()
        {
            return db.GetDate();
        }

        /// <summary>
        /// 清除表缓存
        /// </summary>
        public void RemoveCache()
        {
            var cacheService = db.CurrentConnectionConfig.ConfigureExternalServices.DataInfoCacheService;
            string tableName = db.EntityMaintenance.GetTableName<T>();
            var keys = cacheService.GetAllKey<string>();
            if (keys != null && keys.Count() > 0)
            {
                foreach (var item in keys)
                {
                    if (item.ToLower().Contains("." + tableName.ToLower() + "."))
                    {
                        cacheService.Remove<string>(item);
                    }
                }
            }
        }

        #endregion 其它
    }
}