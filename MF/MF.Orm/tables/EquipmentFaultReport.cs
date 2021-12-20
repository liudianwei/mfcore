using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备故障报告 表
    /// </summary>
    [SugarTable("fm_equipment_fault_report", "设备故障报告")]
    public class EquipmentFaultReport : BaseEntity
    {
        public EquipmentFaultReport()
        {
        }
        
        /// <summary>
        /// Desc:故障报告编号 自动生成
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "report_code", ColumnDescription = "故障报告编号 自动生成", IsNullable = false, DefaultValue = "", Length = 64)]
        public string ReportCode{ get; set; }

        /// <summary>
        /// Desc:设备编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "equipment_code", ColumnDescription = "设备编码", IsNullable = true, Length = 64)]
        public string EquipmentCode{ get; set; }

        /// <summary>
        /// Desc:设备名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "equipment_name", ColumnDescription = "设备名称", IsNullable = true, Length = 64)]
        public string EquipmentName{ get; set; }

        /// <summary>
        /// Desc:维修记录编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_code", ColumnDescription = "维修记录编号", IsNullable = true, Length = 255)]
        public string RepairCode{ get; set; }

        /// <summary>
        /// Desc:设备故障代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_code", ColumnDescription = "设备故障代码", IsNullable = true, Length = 10)]
        public string FaultCode{ get; set; }

        /// <summary>
        /// Desc:影响状况
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "impact_condition", ColumnDescription = "影响状况", IsNullable = true, Length = 500)]
        public string ImpactCondition{ get; set; }

        /// <summary>
        /// Desc:设备机构图
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "equipment_org_file", ColumnDescription = "设备机构图", IsNullable = true, Length = 100)]
        public string EquipmentOrgFile{ get; set; }

        /// <summary>
        /// Desc:处理说明
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "process_info", ColumnDescription = "处理说明", IsNullable = true, Length = 500)]
        public string ProcessInfo{ get; set; }

        /// <summary>
        /// Desc:原因分析图
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "reason_analysis_file", ColumnDescription = "原因分析图", IsNullable = true, Length = 100)]
        public string ReasonAnalysisFile{ get; set; }

        /// <summary>
        /// Desc:图片分析
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "image_analysis_file", ColumnDescription = "图片分析", IsNullable = true, Length = 100)]
        public string ImageAnalysisFile{ get; set; }

        /// <summary>
        /// Desc:再发防止
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "recurrence_prevention", ColumnDescription = "再发防止", IsNullable = true, Length = 100)]
        public string RecurrencePrevention{ get; set; }

        /// <summary>
        /// Desc:附件
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "att_file", ColumnDescription = "附件", IsNullable = true, Length = 100)]
        public string AttFile{ get; set; }

    }
}