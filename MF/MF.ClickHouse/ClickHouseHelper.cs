using ClickHouse.Client.ADO;
using System;

namespace MF.ClickHouse
{
    public class ClickHouseHelper : IDisposable
    {
        private ClickHouseConnection _clickHouseConnection; 
        public ClickHouseHelper()
        {

        }
        public ClickHouseHelper(ClickHouseConnection connectionString) : this()
        {
            _clickHouseConnection = connectionString;
        }

        public void Dispose()
        {
            _clickHouseConnection?.Dispose();
            _clickHouseConnection = null;
            GC.Collect();
        }
    }
}