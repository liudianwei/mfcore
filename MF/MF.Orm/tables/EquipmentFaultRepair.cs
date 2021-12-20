using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备故障维修记录 表
    /// </summary>
    [SugarTable("fm_equipment_fault_repair", "设备故障维修记录")]
    public class EquipmentFaultRepair : BaseEntity
    {
        public EquipmentFaultRepair()
        {
        }
        
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
        [SugarColumn(ColumnName = "fault_code", ColumnDescription = "设备故障代码", IsNullable = true, Length = 255)]
        public string FaultCode{ get; set; }

        /// <summary>
        /// Desc:故障开始时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_start_time", ColumnDescription = "故障开始时间", IsNullable = true, Length = 0)]
        public System.DateTime? FaultStartTime{ get; set; }

        /// <summary>
        /// Desc:故障结束时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_end_time", ColumnDescription = "故障结束时间", IsNullable = true, Length = 0)]
        public System.DateTime? FaultEndTime{ get; set; }

        /// <summary>
        /// Desc:维修状态 0无需维修 1待维修 2正在维修 3维修完成
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_status", ColumnDescription = "维修状态 0无需维修 1待维修 2正在维修 3维修完成", IsNullable = true, Length = 10)]
        public string RepairStatus{ get; set; }

        /// <summary>
        /// Desc:备品料号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "parts_code", ColumnDescription = "备品料号", IsNullable = true, Length = 50)]
        public string PartsCode{ get; set; }

        /// <summary>
        /// Desc:报修时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_call_time", ColumnDescription = "报修时间", IsNullable = true, Length = 0)]
        public System.DateTime? RepairCallTime{ get; set; }

        /// <summary>
        /// Desc:维修开始时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_start_time", ColumnDescription = "维修开始时间", IsNullable = true, Length = 0)]
        public System.DateTime? RepairStartTime{ get; set; }

        /// <summary>
        /// Desc:维修结束时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_end_time", ColumnDescription = "维修结束时间", IsNullable = true, Length = 0)]
        public System.DateTime? RepairEndTime{ get; set; }

        /// <summary>
        /// Desc:维修员工
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_user", ColumnDescription = "维修员工", IsNullable = true, Length = 50)]
        public string RepairUser{ get; set; }

        /// <summary>
        /// Desc:原因分析
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "reason_analysis", ColumnDescription = "原因分析", IsNullable = true, Length = 255)]
        public string ReasonAnalysis{ get; set; }

        /// <summary>
        /// Desc:故障现象描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_desc", ColumnDescription = "故障现象描述", IsNullable = true, Length = 255)]
        public string FaultDesc{ get; set; }

        /// <summary>
        /// Desc:解决方案 维修措施
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_solution", ColumnDescription = "解决方案 维修措施", IsNullable = true, Length = 255)]
        public string FaultSolution{ get; set; }

    }
}