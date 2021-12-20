using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MF.Orm.Repository;

namespace MF.Orm.Service
{
    public class BaseServices<T> : IBaseServices<T> where T : class, new()
    {
        public IBaseRepository<T> _baseRepository;
        //public IBaseRepository<T> _baseRepository = ServiceResolve.Resolve<IBaseRepository<T>>();

        public BaseServices(IBaseRepository<T> repository)
        {
            _baseRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        #region add

        public bool Insert(T t, bool IgnoreNullColumn = true)
        {
            return _baseRepository.Insert(t, IgnoreNullColumn);
        }

        public bool InsertIgnoreNullColumn(T t)
        {
            return _baseRepository.InsertIgnoreNullColumn(t);
        }

        public bool InsertIgnoreNullColumn(T t, params string[] columns)
        {
            return _baseRepository.InsertIgnoreNullColumn(t, columns);
        }

        public bool Insert(SqlSugarClient client, T t)
        {
            return _baseRepository.Insert(client, t);
        }

        public bool Insert(List<T> t)
        {
            return _baseRepository.Insert(t);
        }

        //public bool Saveable(List<T> t)
        //{
        //    return _baseRepository.Saveable(t);
        //}

        public bool InsertIgnoreNullColumn(List<T> t)
        {
            return _baseRepository.InsertIgnoreNullColumn(t);
        }

        public bool InsertIgnoreNullColumn(List<T> t, params string[] columns)
        {
            return _baseRepository.InsertIgnoreNullColumn(t, columns);
        }

        public long InsertBigIdentity(T t)
        {
            return _baseRepository.InsertBigIdentity(t);
        }

        public T InsertReturnEntity(T t)
        {
            return _baseRepository.InsertReturnEntity(t);
        }

        public T InsertReturnEntity(T t, string sqlWith = SqlWith.UpdLock)
        {
            return _baseRepository.InsertReturnEntityWithLock(t, sqlWith);
        }

        public DbResult<bool> InsertTran(T t)
        {
            return _baseRepository.InsertTran(t);
        }

        public DbResult<bool> InsertTran(List<T> t)
        {
            return _baseRepository.InsertTran(t);
        }

        public bool ExecuteCommand(string sql, object parameters)
        {
            return _baseRepository.ExecuteCommand(sql, parameters);
        }

        public bool ExecuteCommand(string sql, params SugarParameter[] parameters)
        {
            return _baseRepository.ExecuteCommand(sql, parameters);
        }

        public bool ExecuteCommand(string sql, List<SugarParameter> parameters)
        {
            return _baseRepository.ExecuteCommand(sql, parameters);
        }

        #endregion add

        #region update

        public bool UpdateEntity(T entity)
        {
            return _baseRepository.UpdateEntity(entity);
        }

        public bool Update(T entity, Expression<Func<T, bool>> expression)
        {
            return _baseRepository.Update(entity, expression);
        }

        public bool Update(T entity, Expression<Func<T, object>> expression)
        {
            return _baseRepository.Update(entity, expression);
        }

        public bool Update(T entity, Expression<Func<T, object>> expression, Expression<Func<T, bool>> where)
        {
            return _baseRepository.Update(entity, expression, where);
        }

        public bool Update(SqlSugarClient client, T entity, Expression<Func<T, object>> expression, Expression<Func<T, bool>> where)
        {
            return _baseRepository.Update(client, entity, expression, where);
        }

        public bool Update(T entity, List<string> list, bool isNull = true)
        {
            return _baseRepository.Update(entity, list, isNull);
        }

        public bool Update(List<T> entity)
        {
            return _baseRepository.Update(entity);
        }

        #endregion update

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public DbResult<bool> UseTran(Action action)
        {
            var result = _baseRepository.UseTran(() => action());
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DbResult<bool> UseTran(SqlSugarClient client, Action action)
        {
            return _baseRepository.UseTran(client, () => action());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool UseTran2(Action action)
        {
            var result = _baseRepository.UseTran2(() => action());
            return result;
        }

        #region delete

        public bool Delete(Expression<Func<T, bool>> expression)
        {
            return _baseRepository.Delete(expression);
        }

        public bool Delete<PkType>(string[] primaryKeyValues)
        {
            return _baseRepository.Delete(primaryKeyValues);
        }

        public bool Delete(T obj)
        {
            return _baseRepository.Delete(obj);
        }

        #endregion delete

        #region query

        public bool IsAny(Expression<Func<T, bool>> expression)
        {
            return _baseRepository.IsAny(expression);
        }

        public ISugarQueryable<T> Queryable()
        {
            return _baseRepository.Queryable();
        }

        public ISugarQueryable<ExpandoObject> Queryable(string tableName, string shortName)
        {
            return _baseRepository.Queryable(tableName, shortName);
        }

        public T QueryableToEntity(Expression<Func<T, bool>> expression)
        {
            return _baseRepository.QueryableToEntity(expression);
        }

        public List<T> QueryableToList(Expression<Func<T, bool>> expression)
        {
            return _baseRepository.QueryableToList(expression);
        }

        public List<T> QueryableToList(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderFiled, OrderByType type = OrderByType.Asc)
        {
            return _baseRepository.QueryableToList(expression, orderFiled, type);
        }

        public Task<List<T>> QueryableToListAsync(Expression<Func<T, bool>> expression)
        {
            return _baseRepository.QueryableToListAsync(expression);
        }

        public List<T> QueryableToList(string tableName)
        {
            return _baseRepository.QueryableToList(tableName);
        }

        public List<T> QueryableToList(string tableName, Expression<Func<T, bool>> expression)
        {
            return _baseRepository.QueryableToList(tableName, expression);
        }

        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> expression, int pageIndex = 0, int pageSize = 10)
        {
            return _baseRepository.QueryableToPage(expression, pageIndex, pageSize);
        }

        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> expression, string order, int pageIndex = 0, int pageSize = 10)
        {
            return _baseRepository.QueryableToPage(expression, order, pageIndex, pageSize);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="orderFiled"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public (List<T>, int) QueryableToPage(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderFiled, string orderBy, int pageIndex = 0, int pageSize = 10)
        {
            //仅针对bootstrap table
            //if (pageIndex != 0)
            //{
            //    pageIndex = pageIndex / pageSize + 1;
            //}
            return _baseRepository.QueryableToPage(expression, orderFiled, orderBy, pageIndex, pageSize);
        }

        public List<T> SqlQueryToList(string sql, object obj = null)
        {
            return _baseRepository.SqlQueryToList(sql, obj);
        }

        #endregion query

        public DataTable UseStoredProcedureToDataTable(string procedureName, List<SugarParameter> parameters)
        {
            return _baseRepository.UseStoredProcedureToDataTable(procedureName, parameters);
        }

        public (DataTable, List<SugarParameter>) UseStoredProcedureToTuple(string procedureName, List<SugarParameter> parameters)
        {
            return _baseRepository.UseStoredProcedureToTuple(procedureName, parameters);
        }
    }
}