using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备保养计划执行时刻表 表
    /// </summary>
    [SugarTable("fm_equipment_upkeep_schedule", "设备保养计划执行时刻表")]
    public class EquipmentUpkeepSchedule : BaseEntity
    {
        public EquipmentUpkeepSchedule()
        {
        }
        
        /// <summary>
        /// Desc:产线代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "产线代码", IsNullable = true, Length = 64)]
        public string LineCode{ get; set; }

        /// <summary>
        /// Desc:产线名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线名称", IsNullable = true, Length = 64)]
        public string LineName{ get; set; }

        /// <summary>
        /// Desc:计划主表id
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "plan_id", ColumnDescription = "计划主表id", IsNullable = false, DefaultValue = "", Length = 36)]
        public string PlanId{ get; set; }

        /// <summary>
        /// Desc:计划类型编码 班次、天、周、月、年、季、半年
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plan_type_code", ColumnDescription = "计划类型编码 班次、天、周、月、年、季、半年", IsNullable = true, Length = 64)]
        public string PlanTypeCode{ get; set; }

        /// <summary>
        /// Desc:计划类型名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plan_type_name", ColumnDescription = "计划类型名称", IsNullable = true, Length = 64)]
        public string PlanTypeName{ get; set; }

        /// <summary>
        /// Desc:工位名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 100)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:保养计划执行时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "do_time", ColumnDescription = "保养计划执行时间", IsNullable = true, Length = 0)]
        public System.DateTime? DoTime{ get; set; }

        /// <summary>
        /// Desc:保养状态 0未保养 1保养中 2已保养
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_finish", ColumnDescription = "保养状态 0未保养 1保养中 2已保养", IsNullable = true, Length = 10)]
        public string IsFinish{ get; set; }

        /// <summary>
        /// Desc:实际完成时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "finish_time", ColumnDescription = "实际完成时间", IsNullable = true, Length = 0)]
        public System.DateTime? FinishTime{ get; set; }

        /// <summary>
        /// Desc:保养内容id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "content_id", ColumnDescription = "保养内容id", IsNullable = true, Length = 36)]
        public string ContentId{ get; set; }

        /// <summary>
        /// Desc:保养时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upkeep_time", ColumnDescription = "保养时间", IsNullable = true, Length = 0)]
        public System.DateTime? UpkeepTime{ get; set; }

        /// <summary>
        /// Desc:保养人
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operator", ColumnDescription = "保养人", IsNullable = true, Length = 64)]
        public string Operator{ get; set; }

        /// <summary>
        /// Desc:保养说明
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "保养说明", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}