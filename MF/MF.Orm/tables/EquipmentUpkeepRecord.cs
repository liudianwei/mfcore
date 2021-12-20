using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备保养记录 表
    /// </summary>
    [SugarTable("fm_equipment_upkeep_record", "设备保养记录")]
    public class EquipmentUpkeepRecord : BaseEntity
    {
        public EquipmentUpkeepRecord()
        {
        }
        
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