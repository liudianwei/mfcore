using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 安东呼叫综合呼叫表 表
    /// </summary>
    [SugarTable("fm_andon_general_call", "安东呼叫综合呼叫表")]
    public class AndonGeneralCall : BaseEntity
    {
        public AndonGeneralCall()
        {
        }
        
        /// <summary>
        /// Desc:呼叫记录id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "record_id", ColumnDescription = "呼叫记录id", IsNullable = true, Length = 36)]
        public string RecordId{ get; set; }

        /// <summary>
        /// Desc:安东处理明细id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "handle_id", ColumnDescription = "安东处理明细id", IsNullable = true, Length = 36)]
        public string HandleId{ get; set; }

        /// <summary>
        /// Desc:安东明细编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_detail_code", ColumnDescription = "安东明细编号", IsNullable = true, Length = 64)]
        public string AndonDetailCode{ get; set; }

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
        /// Desc:安东类型,维护到数据字典里
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_type", ColumnDescription = "安东类型,维护到数据字典里", IsNullable = true, Length = 64)]
        public string AndonType{ get; set; }

        /// <summary>
        /// Desc:呼叫状态 1呼叫 2响应 3解除
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_status", ColumnDescription = "呼叫状态 1呼叫 2响应 3解除", IsNullable = true, Length = 100)]
        public string CallStatus{ get; set; }

        /// <summary>
        /// Desc:安东处理明细应答周期
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "answer_time", ColumnDescription = "安东处理明细应答周期", IsNullable = true, Length = 11)]
        public int? AnswerTime{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}