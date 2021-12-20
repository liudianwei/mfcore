using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// LED管理 表
    /// </summary>
    [SugarTable("fm_led_info", "LED管理")]
    public class LedInfo : BaseEntity
    {
        public LedInfo()
        {
        }
        
        /// <summary>
        /// Desc:LED编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "led_num", ColumnDescription = "LED编号", IsNullable = true, Length = 10)]
        public string LedNum{ get; set; }

        /// <summary>
        /// Desc:线体类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_type", ColumnDescription = "线体类型", IsNullable = true, Length = 10)]
        public string LineType{ get; set; }

        /// <summary>
        /// Desc:标题
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "title", ColumnDescription = "标题", IsNullable = true, Length = 100)]
        public string Title{ get; set; }

        /// <summary>
        /// Desc:滚动栏
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "scroll_bar_info", ColumnDescription = "滚动栏", IsNullable = true, Length = 100)]
        public string ScrollBarInfo{ get; set; }

        /// <summary>
        /// Desc:通知
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "notice", ColumnDescription = "通知", IsNullable = true, Length = 100)]
        public string Notice{ get; set; }

        /// <summary>
        /// Desc:日计划产量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "day_plan_output", ColumnDescription = "日计划产量", IsNullable = true, Length = 11)]
        public int? DayPlanOutput{ get; set; }

        /// <summary>
        /// Desc:日实际产量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "day_actual_output", ColumnDescription = "日实际产量", IsNullable = true, Length = 11)]
        public int? DayActualOutput{ get; set; }

        /// <summary>
        /// Desc:月计划产量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "month_plan_output", ColumnDescription = "月计划产量", IsNullable = true, Length = 11)]
        public int? MonthPlanOutput{ get; set; }

        /// <summary>
        /// Desc:月实际产量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "month_actual_output", ColumnDescription = "月实际产量", IsNullable = true, Length = 11)]
        public int? MonthActualOutput{ get; set; }

        /// <summary>
        /// Desc:当前机型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "current_engine_type", ColumnDescription = "当前机型", IsNullable = true, Length = 64)]
        public string CurrentEngineType{ get; set; }

        /// <summary>
        /// Desc:上线工位
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "online_opname", ColumnDescription = "上线工位", IsNullable = true, Length = 64)]
        public string OnlineOpname{ get; set; }

        /// <summary>
        /// Desc:下线工位
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "offline_opname", ColumnDescription = "下线工位", IsNullable = true, Length = 64)]
        public string OfflineOpname{ get; set; }

    }
}