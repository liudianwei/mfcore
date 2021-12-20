using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备故障代码存储库 表
    /// </summary>
    [SugarTable("fm_equipment_fault_code", "设备故障代码存储库")]
    public class EquipmentFaultCode : BaseEntity
    {
        public EquipmentFaultCode()
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
        /// Desc:故障代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_code", ColumnDescription = "故障代码", IsNullable = true, Length = 255)]
        public string FaultCode{ get; set; }

        /// <summary>
        /// Desc:故障描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_desc", ColumnDescription = "故障描述", IsNullable = true, Length = 255)]
        public string FaultDesc{ get; set; }

        /// <summary>
        /// Desc:解决方案
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_solution", ColumnDescription = "解决方案", IsNullable = true, Length = 255)]
        public string FaultSolution{ get; set; }

    }
}