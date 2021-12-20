using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 报警报文维护 表
    /// </summary>
    [SugarTable("fm_equipment_alarm_message", "报警报文维护")]
    public class EquipmentAlarmMessage : BaseEntity
    {
        public EquipmentAlarmMessage()
        {
        }
        
        /// <summary>
        /// Desc:工位号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号", IsNullable = true, Length = 64)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 100)]
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
        /// Desc:PLC地址说明
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plc_address", ColumnDescription = "PLC地址说明", IsNullable = true, Length = 255)]
        public string PlcAddress{ get; set; }

        /// <summary>
        /// Desc:变量位
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tag_bit", ColumnDescription = "变量位", IsNullable = true, Length = 11)]
        public int? TagBit{ get; set; }

        /// <summary>
        /// Desc:报文类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "msg_type", ColumnDescription = "报文类型", IsNullable = true, Length = 50)]
        public string MsgType{ get; set; }

        /// <summary>
        /// Desc:报警内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "msg_content", ColumnDescription = "报警内容", IsNullable = true, Length = 255)]
        public string MsgContent{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}