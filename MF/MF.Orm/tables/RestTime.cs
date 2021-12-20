using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 休息时间管理 表
    /// </summary>
    [SugarTable("fm_rest_time", "休息时间管理")]
    public class RestTime : BaseEntity
    {
        public RestTime()
        {
        }
        
        /// <summary>
        /// Desc:休息日期
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "rest_date", ColumnDescription = "休息日期", IsNullable = true, Length = 0)]
        public System.DateTime? RestDate{ get; set; }

        /// <summary>
        /// Desc:产线code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "产线code", IsNullable = true, Length = 36)]
        public string LineCode{ get; set; }

        /// <summary>
        /// Desc:产线name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线name", IsNullable = true, Length = 64)]
        public string LineName{ get; set; }

        /// <summary>
        /// Desc:时段名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "period_name", ColumnDescription = "时段名称", IsNullable = true, Length = 200)]
        public string PeriodName{ get; set; }

        /// <summary>
        /// Desc:休息起始时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "start_time", ColumnDescription = "休息起始时间", IsNullable = true, Length = 8)]
        public string StartTime{ get; set; }

        /// <summary>
        /// Desc:休息截止时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "end_time", ColumnDescription = "休息截止时间", IsNullable = true, Length = 8)]
        public string EndTime{ get; set; }

        /// <summary>
        /// Desc:休息时长（分钟）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "rest_duration", ColumnDescription = "休息时长（分钟）", IsNullable = true, Length = 10)]
        public string RestDuration{ get; set; }

        /// <summary>
        /// Desc:休息类型　1通用2特殊
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "rest_type", ColumnDescription = "休息类型　1通用2特殊", IsNullable = false, DefaultValue = "1", Length = 10)]
        public string RestType{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remarks", ColumnDescription = "备注", IsNullable = true, Length = 500)]
        public string Remarks{ get; set; }

    }
}