using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备保养计划 表
    /// </summary>
    [SugarTable("fm_equipment_upkeep_plan", "设备保养计划")]
    public class EquipmentUpkeepPlan : BaseEntity
    {
        public EquipmentUpkeepPlan()
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
        /// Desc:(班次、月)对应的选类型编码;
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "type_code", ColumnDescription = "(班次、月)对应的选类型编码;", IsNullable = true, Length = 50)]
        public string TypeCode{ get; set; }

        /// <summary>
        /// Desc:名称(班次、月)
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "type_name", ColumnDescription = "名称(班次、月)", IsNullable = true, Length = 50)]
        public string TypeName{ get; set; }

        /// <summary>
        /// Desc:保养时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upkeep_time", ColumnDescription = "保养时间", IsNullable = true, Length = 0)]
        public System.DateTime? UpkeepTime{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}