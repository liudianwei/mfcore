using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 班次管理 表
    /// </summary>
    [SugarTable("fm_shift", "班次管理")]
    public class Shift : BaseEntity
    {
        public Shift()
        {
        }
        
        /// <summary>
        /// Desc:班次编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_code", ColumnDescription = "班次编码", IsNullable = true, Length = 100)]
        public string ShiftCode{ get; set; }

        /// <summary>
        /// Desc:班次名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_name", ColumnDescription = "班次名称", IsNullable = true, Length = 100)]
        public string ShiftName{ get; set; }

        /// <summary>
        /// Desc:所属产线代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "所属产线代码", IsNullable = true, Length = 36)]
        public string LineCode{ get; set; }

        /// <summary>
        /// Desc:上班时间 HH:MM:SS
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "start_time", ColumnDescription = "上班时间 HH:MM:SS", IsNullable = true, Length = 8)]
        public string StartTime{ get; set; }

        /// <summary>
        /// Desc:下班时间 HH:MM:SS
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "end_time", ColumnDescription = "下班时间 HH:MM:SS", IsNullable = true, Length = 8)]
        public string EndTime{ get; set; }

        /// <summary>
        /// Desc:是否跨天 ０否1是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_day_span", ColumnDescription = "是否跨天 ０否1是", IsNullable = true, Length = 10)]
        public string IsDaySpan{ get; set; }

    }
}