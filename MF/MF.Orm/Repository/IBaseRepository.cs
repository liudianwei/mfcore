using SqlSugar;

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MF.Ioc;

namespace MF.Orm.Repository
{
    public interface IBaseRepository<T> : IDependency where T : class, new()
    {
        #region 新增

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="IgnoreNullColumn">默认true 忽略空字段， false 不忽略空字段</param>
        /// <returns>true 成功， false 失败</returns>
        bool Insert(T entity, bool IgnoreNullColumn = true);

        /// <summary>
        /// 插入一条记录 自动忽略空字段
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功， false 失败</returns>
        bool InsertIgnoreNullColumn(T entity);

        /// <summary>
        /// 插入一条记录 忽略指定的列
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略的列数组</param>
        /// <returns>true 成功，false 失败</returns>
        bool InsertIgnoreNullColumn(T entity, params string[] ignoreColumns);

        /// <summary>
        /// 插入一条记录 指定使用数据库实例
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        bool Insert(SqlSugarClient _db, T entity);
        /// <summary>
        /// 大数据写入 bulk插入
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool BulkCopy(List<T> entitys);

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>大数主键</returns>
        long InsertBigIdentity(T entity);

        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        bool Insert(List<T> entitys);

        /// <summary>
        /// 插入多条记录 自动忽略空的列
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        bool InsertIgnoreNullColumn(List<T> entitys);

        /// <summary>
        /// 插入多条记录 自动忽略空的列
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        bool Insert(List<T> entitys, out List<string> ids);

        /// <summary>
        /// 插入多条记录 指定忽略的列集合
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <param name="ignoreColumns">忽略的列数组</param>
        /// <returns>true 成功，false 失败</returns>
        bool InsertIgnoreNullColumn(List<T> entitys, params string[] ignoreColumns);

        /// <summary>
        /// 使用事务 插入一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        DbResult<bool> InsertTran(T entity);

        /// <summary>
        /// 使用事务 插入多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        DbResult<bool> InsertTran(List<T> entitys);

        /// <summary>
        /// 插入一条记录 返回记录对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>刚插入的记录对象</returns>
        T InsertReturnEntity(T entity);

        /// <summary>
        /// 使用数据库锁参数 插入一条记录 返回记录对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="sqlWith">锁参数</param>
        /// <returns>刚插入的记录对象</returns>
        T InsertReturnEntityWithLock(T entity, string sqlWith = SqlWith.UpdLock);

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        bool ExecuteCommand(string sql, object parameters);

        /// <summary>
        /// 执行sql语句 指定参数列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        bool ExecuteCommand(string sql, params SugarParameter[] parameters);

        /// <summary>
        /// 执行sql语句 指定参数列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>true 成功，false 失败</returns>
        bool ExecuteCommand(string sql, List<SugarParameter> parameters);

        #endregion 新增

        #region 更新

        /// <summary>
        /// 大数据写入 bulk插入
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool BulkUpdate(List<T> entitys);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true 成功，false 失败</returns>
        bool UpdateEntity(T entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="where">where条件</param>
        /// <returns>true 成功，false 失败</returns>
        bool Update(T entity, Expression<Func<T, bool>> where);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="where">where条件</param>
        /// <returns>true 成功，false 失败</returns>
        /// <returns></returns>
        bool Update(T entity, Expression<Func<T, object>> where);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="updateColumns">更新的列</param>
        /// <param name="where">where条件</param>
        /// <returns>true 成功，false 失败</returns>
        bool Update(T entity, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> where);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="entity">实体对象</param>
        /// <param name="updateColumns">更新的列 表达式</param>
        /// <param name="where">where表达式</param>
        /// <returns>true 成功，false 失败</returns>
        bool Update(SqlSugarClient _db, T entity, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> where);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新的实体</param>
        /// <param name="ignores">忽略更新的列列表</param>
        /// <param name="isNull">是否忽略更新 默认为true</param>
        /// <returns>true 成功，false 失败</returns>
        bool Update(T entity, List<string> ignores = null, bool isNull = true);

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="entitys">实体对象列表</param>
        /// <returns>true 成功，false 失败</returns>
        bool Update(List<T> entity);

        #endregion 更新

        #region 事务

        /// <summary>
        ///
        /// </summary>
        /// <param name="action">事务内容</param>
        /// <returns>true 成功，false 失败</returns>
        DbResult<bool> UseTran(Action action);

        /// <summary>
        ///
        /// </summary>
        /// <param name="client">sugar 数据库实例</param>
        /// <param name="action">事务内容</param>
        /// <returns>true 成功，false 失败</returns>
        DbResult<bool> UseTran(SqlSugarClient client, Action action);

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <returns>true 成功，false 失败</returns>
        bool UseTran2(Action action);

        #endregion 事务

        #region 删除

        /// <summary>
        /// 根据主键列表删除记录 默认逻辑删除
        /// </summary>
        /// <param name="primaryKeys">主键数组</param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        bool Delete(string[] primaryKeys, bool logic = true);

        /// <summary>
        /// 删除记录 默认逻辑删除
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        bool Delete(Expression<Func<T, bool>> where, bool logic = true);

        /// <summary>
        /// 根据对象删除记录 默认逻辑删除
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="logic">默认true 表示逻辑删除, false 表示物理删除</param>
        /// <returns>true 成功，false 失败</returns>
        bool Delete(T obj, bool logic = true);

        #endregion 删除

        #region 查询

        /// <summary>
        /// 判断记录是否存在 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>true 成功，false 失败</returns>
        bool IsAny(Expression<Func<T, bool>> where, bool logic = true);

        /// <summary>
        /// 查询 默认逻辑查询
        /// </summary>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <param name="_tenantCode">租户代码</param>
        /// <returns>查询对象</returns>
        ISugarQueryable<T> Queryable(bool logic = true);

        /// <summary>
        /// 根据表名查询记录 只有物理查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="shortName">别名</param>
        /// <returns>动态的查询对象</returns>
        ISugarQueryable<ExpandoObject> Queryable(string tableName, string shortName);

        /// <summary>
        /// 查询多条记录 默认逻辑查询 查询所有数据
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        List<T> QueryAll(bool logic = true);

        /// <summary>
        /// 异步逻辑理查询 默认逻辑查询 查询所有数据
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        Task<List<T>> QueryableToListAsync(bool logic = true);

        /// <summary>
        /// 查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        List<T> QueryableToList(Expression<Func<T, bool>> where, bool logic = true);

        /// <summary>
        /// 查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderBy"></param>
        /// <param name="ordinal"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        List<T> QueryableToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, OrderByType ordinal = OrderByType.Asc, bool logic = true);

        /// <summary>
        /// 异步查询多条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        Task<List<T>> QueryableToListAsync(Expression<Func<T, bool>> where, bool logic = true);

        /// <summary>
        /// 根据指定表名称查询记录列表 默认逻辑查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        List<T> QueryableToList(string tableName, bool logic = true);

        /// <summary>
        /// 查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <param name="_tenantId">租户代码</param>
        /// <returns>记录对象</returns>
        T QueryableToEntity(Expression<Func<T, bool>> where, bool logic = true);

        /// <summary>
        /// 异步查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        Task<T> QueryableToEntityAsync(Expression<Func<T, bool>> where, bool logic = true);

        /// <summary>
        /// 查询一条记录 默认逻辑查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderBy"></param>
        /// <param name="ordinal"></param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录对象</returns>
        T QueryableToEntity(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, OrderByType ordinal = OrderByType.Desc, bool logic = true);

        /// <summary>
        /// 根据指定表名称 和 where条件 查询记录列表 默认逻辑查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">where表达式</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表</returns>
        List<T> QueryableToList(string tableName, Expression<Func<T, bool>> where, bool logic = true);

        /// <summary>
        /// 分页查询 返回记录列表和记录总数 默认逻辑查询
        /// </summary>
        /// <param name="where">where表达式</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表 和 记录总数</returns>
        (List<T>, int) QueryableToPage(Expression<Func<T, bool>> where, int pageIndex = 0, int pageSize = 10, bool logic = true);

        /// <summary>
        /// 根据排序条件 分页查询 返回记录列表和记录总数 默认逻辑查询
        /// </summary>
        /// <param name="where">where表达式</param>
        /// <param name="orderBy">排序列</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="logic">默认true 表示逻辑查询, false 表示物理查询</param>
        /// <returns>记录列表 和 记录总数</returns>
        (List<T>, int) QueryableToPage(Expression<Func<T, bool>> where, string orderBy, int pageIndex = 0, int pageSize = 10, bool logic = true);

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
        (List<T>, int) QueryableToPage(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, string ordinal, int pageIndex = 0, int pageSize = 10, bool logic = true);

        /// <summary>
        /// sql原生查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>记录列表</returns>
        List<T> SqlQueryToList(string sql, object parameters = null);

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
        DataTable UseStoredProcedureToDataTable(string procedureName, List<SugarParameter> parameters);

        /// <summary>
        /// 调用存储过程 带output返回值
        /// var list = new List<SugarParameter>();
        /// list.Add(new SugarParameter(ParaName, ParaValue, true));  output
        /// list.Add(new SugarParameter(ParaName, ParaValue)); input
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集 和 返回参数</returns>
        (DataTable, List<SugarParameter>) UseStoredProcedureToTuple(string procedureName, List<SugarParameter> parameters);

        #endregion 存储过程
    }
}