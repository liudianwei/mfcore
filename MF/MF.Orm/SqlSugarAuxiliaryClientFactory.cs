using SqlSugar;
using System;

namespace MF.Orm
{
    internal static class SqlSugarAuxiliaryClientFactory
    {
        internal static SqlSugarClient Create(ISqlSugarClient source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var sourceConfig = source.CurrentConnectionConfig;
            if (sourceConfig == null)
                throw new InvalidOperationException("当前数据库客户端没有连接配置。");

            // SqlSugarClient 拥有独立的 ADO 状态，且不会继承 source 实例上的 AOP 回调。
            // 创建新配置避免修改外层客户端的自动关闭策略；故意不复制 AopEvents。
            var config = new ConnectionConfig
            {
                ConfigId = sourceConfig.ConfigId,
                ConnectionString = sourceConfig.ConnectionString,
                DbLinkName = sourceConfig.DbLinkName,
                DbType = sourceConfig.DbType,
                IndexSuffix = sourceConfig.IndexSuffix,
                InitKeyType = sourceConfig.InitKeyType,
                IsAutoCloseConnection = true,
                LanguageType = sourceConfig.LanguageType,
                MoreSettings = sourceConfig.MoreSettings,
                SlaveConnectionConfigs = sourceConfig.SlaveConnectionConfigs,
                SqlMiddle = sourceConfig.SqlMiddle,
                ConfigureExternalServices = sourceConfig.ConfigureExternalServices
            };
            return new SqlSugarClient(config);
        }
    }
}
