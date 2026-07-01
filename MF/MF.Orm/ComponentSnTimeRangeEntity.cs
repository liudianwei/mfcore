using SqlSugar;
using System;

namespace MF.Orm
{
    [SugarTable("component_sn_time_range", "SN时间范围配置")]
    public class ComponentSnTimeRangeEntity : BaseEntity
    {
        [SugarColumn(ColumnName = "sn", ColumnDescription = "SN", IsNullable = false, Length = 128)]
        public string Sn { get; set; }

        [SugarColumn(ColumnName = "start_time", ColumnDescription = "起始时间", IsNullable = true)]
        public DateTime? StartTime { get; set; }

        [SugarColumn(ColumnName = "end_time", ColumnDescription = "截止时间", IsNullable = true)]
        public DateTime? EndTime { get; set; }
    }
}
