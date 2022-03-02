using System;

using SqlSugar;

namespace MF.Job.Core.Business.Manager
{
    public class DbManager
    {
        public SqlSugarClient db
        {
            get
            {
                var path = Environment.CurrentDirectory;
                var ConnectionStrings = @"DataSource=" + path + @"\job.db";
                var _db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ConnectionStrings, DbType = DbType.Sqlite, IsAutoCloseConnection = true });

                if (_db != null)
                {
                    string BackgroundJobMappingDbTable = "job_task";
                    BackgroundJobMappingDbTable = string.IsNullOrWhiteSpace(BackgroundJobMappingDbTable) ? "job_task" : BackgroundJobMappingDbTable;
                    _db.MappingTables.Add("BackgroundJobInfo", BackgroundJobMappingDbTable);
                }
                var blFlag = false;//是否输出sql日志
                if (blFlag)
                {
                    _db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        foreach (var item in pars)
                        {
                            sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                        }
                        Console.WriteLine($"执行后SQL==>{sql}");
                    };
                    _db.Aop.OnError = (exp) =>//执行SQL 错误事件
                    {
                        Console.WriteLine(exp);
                    };
                }
                return _db;
            }
        }
    }
}