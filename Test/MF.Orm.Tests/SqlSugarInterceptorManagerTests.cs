using Microsoft.Extensions.Logging.Abstractions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MF.Orm.Tests
{
    public class SqlSugarInterceptorManagerTests
    {
        [Fact]
        public void InstanceInterceptors_AreIsolatedPerClient()
        {
            using var first = CreateClient();
            using var second = CreateClient();

            SqlSugarInterceptorManager.RegisterInterceptor(first,
                (sql, pars) => KeyValuePair.Create(sql + " /* first */", pars), "same-key");
            SqlSugarInterceptorManager.RegisterInterceptor(second,
                (sql, pars) => KeyValuePair.Create(sql + " /* second */", pars), "same-key");
            first.InitPipeline();
            second.InitPipeline();

            string firstSql = null;
            string secondSql = null;
            first.Aop.OnLogExecuting = (sql, _) => firstSql = sql;
            second.Aop.OnLogExecuting = (sql, _) => secondSql = sql;

            Assert.Equal(1, first.Ado.GetInt("SELECT 1"));
            Assert.Equal(1, second.Ado.GetInt("SELECT 1"));
            Assert.Contains("/* first */", firstSql);
            Assert.DoesNotContain("/* second */", firstSql);
            Assert.Contains("/* second */", secondSql);
            Assert.DoesNotContain("/* first */", secondSql);
        }

        [Fact]
        public void RegisterInterceptor_IgnoresDuplicateKeyOnSameClient()
        {
            using var db = CreateClient();
            var calls = 0;
            SqlSugarInterceptorManager.RegisterInterceptor(db,
                (sql, pars) =>
                {
                    calls++;
                    return KeyValuePair.Create(sql, pars);
                }, "duplicate");
            SqlSugarInterceptorManager.RegisterInterceptor(db,
                (sql, pars) =>
                {
                    calls += 100;
                    return KeyValuePair.Create(sql, pars);
                }, "duplicate");
            db.InitPipeline();

            Assert.Equal(1, db.Ado.GetInt("SELECT 1"));
            Assert.Equal(1, calls);
        }

        [Fact]
        public void AuxiliaryClient_DoesNotInheritSourceAopAndDoesNotMutateSourceConfig()
        {
            using var source = CreateClient(isAutoCloseConnection: false);
            source.Aop.OnExecutingChangeSql = (sql, pars) =>
                KeyValuePair.Create("SELECT invalid_column", pars);

            using var auxiliary = SqlSugarAuxiliaryClientFactory.Create(source);

            Assert.False(source.CurrentConnectionConfig.IsAutoCloseConnection);
            Assert.True(auxiliary.CurrentConnectionConfig.IsAutoCloseConnection);
            Assert.Equal(1, auxiliary.Ado.GetInt("SELECT 1"));
        }

        [Fact]
        public void PartitionInterceptor_UsesAuxiliaryConnectionAndAppendsTimeBounds()
        {
            var path = CreateDatabasePath();
            try
            {
                using var db = CreateFileClient(path);
                CreateTables(db);
                db.Ado.ExecuteCommand(
                    "INSERT INTO component_sn_time_range " +
                    "(id, component_sn, start_time, end_time, state, inner_version) " +
                    "VALUES ('1', 'SN001', '2026-10-23 10:00:00', '2026-10-23 11:00:00', '0', 0)");
                db.Ado.ExecuteCommand(
                    "INSERT INTO partition_test (id, component_sn, create_time) " +
                    "VALUES ('1', 'SN001', '2026-10-23 10:30:00')");
                db.UseAutoPartitionKeyByComponentSn(NullLogger.Instance);
                db.InitPipeline();
                string executedSql = null;
                db.Aop.OnLogExecuting = (sql, _) => executedSql = sql;

                var result = db.Queryable<PartitionTestEntity>()
                    .Where(item => item.ComponentSn == "SN001")
                    .ToList();

                Assert.Single(result);
                Assert.Contains("create_time >= '2026-10-23 09:59:00'", executedSql);
                Assert.Contains("create_time <= '2026-10-23 11:01:00'", executedSql);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void TrackerInterceptor_UsesAuxiliaryConnectionAndDoesNotReenterPipeline()
        {
            var path = CreateDatabasePath();
            try
            {
                using var db = CreateFileClient(path);
                CreateTables(db);
                db.UseAutoSnTimeRangeTracker(NullLogger.Instance);
                db.InitPipeline();

                var affected = db.Insertable(new PartitionTestEntity
                {
                    Id = "2",
                    ComponentSn = "SN002",
                    CreateTime = new DateTime(2026, 10, 23, 12, 0, 0)
                }).ExecuteCommand();

                var range = db.Queryable<ComponentSnTimeRangeEntity>()
                    .Where(item => item.ComponentSn == "SN002")
                    .Single();
                Assert.Equal(1, affected);
                Assert.NotNull(range);
                Assert.Equal(new DateTime(2026, 10, 23, 12, 0, 0), range.StartTime);
            }
            finally
            {
                File.Delete(path);
            }
        }

        private static SqlSugarClient CreateClient(bool isAutoCloseConnection = true)
        {
            return new SqlSugarClient(new ConnectionConfig
            {
                DbType = DbType.Sqlite,
                ConnectionString = "Data Source=:memory:",
                IsAutoCloseConnection = isAutoCloseConnection,
                InitKeyType = InitKeyType.Attribute
            });
        }

        private static SqlSugarClient CreateFileClient(string path)
        {
            return new SqlSugarClient(new ConnectionConfig
            {
                DbType = DbType.Sqlite,
                ConnectionString = $"Data Source={path};Pooling=False",
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        private static string CreateDatabasePath()
        {
            return Path.Combine(Path.GetTempPath(), $"mf-orm-{Guid.NewGuid():N}.db");
        }

        private static void CreateTables(SqlSugarClient db)
        {
            db.Ado.ExecuteCommand(
                "CREATE TABLE component_sn_time_range (" +
                "id TEXT PRIMARY KEY, component_sn TEXT NOT NULL, start_time TEXT NULL, end_time TEXT NULL, " +
                "creator TEXT NULL, create_time TEXT NULL, updator TEXT NULL, update_time TEXT NULL, " +
                "state TEXT NULL, inner_version INTEGER NOT NULL)");
            db.Ado.ExecuteCommand(
                "CREATE TABLE partition_test (id TEXT PRIMARY KEY, component_sn TEXT NOT NULL, create_time TEXT NULL)");
        }

        [SugarTable("partition_test")]
        private sealed class PartitionTestEntity
        {
            [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
            public string Id { get; set; }

            [SugarColumn(ColumnName = "component_sn")]
            public string ComponentSn { get; set; }

            [SugarColumn(ColumnName = "create_time")]
            public DateTime CreateTime { get; set; }
        }
    }
}
