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
                return _db;
            }
        }
    }
}