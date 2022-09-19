using ClickHouse.Client.ADO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace MF.ClickHouse
{
    public static class ServiceCollectionExt
    {
        private static ClickHouseConnection _clickHouseConnection = null;
        public static Feature SupportedFeatures;


        public static IServiceCollection AddClickHouseClient(this IServiceCollection services, IConfiguration configuration)
        {
            CreateConnection(configuration);
            services.AddSingleton(_clickHouseConnection);
            return services;
        }
        public static ClickHouseConnection CreateConnection(IConfiguration configuration)
        {
            try
            {
                if (_clickHouseConnection == null)
                {
                    var chConnectionString = configuration["Orm:chConnectionString"];
                    if (!string.IsNullOrWhiteSpace(chConnectionString))
                    {
                        using (var cnn = new ClickHouseConnection(chConnectionString))
                        {
                            if (cnn.State != ConnectionState.Open)
                            {
                                cnn.Open();
                            }
                            _clickHouseConnection = cnn;
                            Console.WriteLine("### ClickHouse 连接成功 ###");
                        }
                    }
                    else
                    {
                        _clickHouseConnection = new ClickHouseConnection();
                        Console.WriteLine("### [Orm:chConnectionString] is missing in appsettings.json ###");
                    }
                }
            }
            catch (Exception)
            {
                _clickHouseConnection=new ClickHouseConnection();
                Console.WriteLine("### ClickHouse not connected ###");
            } 
            return _clickHouseConnection;
        }
    }
}
