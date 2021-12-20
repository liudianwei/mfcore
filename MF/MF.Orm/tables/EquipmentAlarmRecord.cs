using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备报警记录 表
    /// </summary>
    [SugarTable("sfc_equipment_alarm_record", "设备报警记录")]
    public class EquipmentAlarmRecord : BaseEntity
    {
        public EquipmentAlarmRecord()
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
        /// Desc:工位号，OP1010
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号，OP1010", IsNullable = true, Length = 36)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述，同步器等件上线
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述，同步器等件上线", IsNullable = true, Length = 255)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:报文id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "msg_id", ColumnDescription = "报文id", IsNullable = true, Length = 36)]
        public string MsgId{ get; set; }

        /// <summary>
        /// Desc:变量id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tag_id", ColumnDescription = "变量id", IsNullable = true, Length = 36)]
        public string TagId{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "msg_type", ColumnDescription = "", IsNullable = true, Length = 60)]
        public string MsgType{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "msg_content", ColumnDescription = "", IsNullable = true, Length = 255)]
        public string MsgContent{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "value", ColumnDescription = "", IsNullable = true, Length = 11)]
        public int? Value{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "", IsNullable = true, Length = 11)]
        public int? Status{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "start_time", ColumnDescription = "", IsNullable = true, Length = 0)]
        public System.DateTime? StartTime{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "end_time", ColumnDescription = "", IsNullable = true, Length = 0)]
        public System.DateTime? EndTime{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "duration", ColumnDescription = "", IsNullable = true, Length = 11)]
        public int? Duration{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_time", ColumnDescription = "", IsNullable = true, Length = 0)]
        public System.DateTime? RepairTime{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_duration", ColumnDescription = "", IsNullable = true, Length = 11)]
        public int? RepairDuration{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_user", ColumnDescription = "", IsNullable = true, Length = 50)]
        public string RepairUser{ get; set; }

        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_sync", ColumnDescription = "", IsNullable = true, Length = 11)]
        public int? IsSync{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}