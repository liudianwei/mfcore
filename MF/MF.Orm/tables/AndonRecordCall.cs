using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 安东呼叫呼叫记录表 表
    /// </summary>
    [SugarTable("fm_andon_record_call", "安东呼叫呼叫记录表")]
    public class AndonRecordCall : BaseEntity
    {
        public AndonRecordCall()
        {
        }
        
        /// <summary>
        /// Desc:安东处理明细id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "handle_id", ColumnDescription = "安东处理明细id", IsNullable = true, Length = 36)]
        public string HandleId{ get; set; }

        /// <summary>
        /// Desc:安东类型,维护到数据字典里
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_type", ColumnDescription = "安东类型,维护到数据字典里", IsNullable = true, Length = 64)]
        public string AndonType{ get; set; }

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
        /// Desc:订单号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单号", IsNullable = true, Length = 64)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:总成唯一编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成唯一编号", IsNullable = true, Length = 255)]
        public string ComponentSn{ get; set; }

        /// <summary>
        /// Desc:安东明细编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_detail_code", ColumnDescription = "安东明细编号", IsNullable = true, Length = 64)]
        public string AndonDetailCode{ get; set; }

        /// <summary>
        /// Desc:安东明细描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_detail_name", ColumnDescription = "安东明细描述", IsNullable = true, Length = 64)]
        public string AndonDetailName{ get; set; }

        /// <summary>
        /// Desc:开始呼叫
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_start", ColumnDescription = "开始呼叫", IsNullable = true, Length = 11)]
        public int? CallStart{ get; set; }

        /// <summary>
        /// Desc:呼叫时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_time", ColumnDescription = "呼叫时间", IsNullable = true, Length = 0)]
        public System.DateTime? CallTime{ get; set; }

        /// <summary>
        /// Desc:呼叫人代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_user_name", ColumnDescription = "呼叫人代码", IsNullable = true, Length = 50)]
        public string CallUserName{ get; set; }

        /// <summary>
        /// Desc:呼叫人姓名
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_user_fullname", ColumnDescription = "呼叫人姓名", IsNullable = true, Length = 50)]
        public string CallUserFullname{ get; set; }

        /// <summary>
        /// Desc:呼叫内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_message", ColumnDescription = "呼叫内容", IsNullable = true, Length = 200)]
        public string CallMessage{ get; set; }

        /// <summary>
        /// Desc:呼叫物料条码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_material_code", ColumnDescription = "呼叫物料条码", IsNullable = true, Length = 50)]
        public string CallMaterialCode{ get; set; }

        /// <summary>
        /// Desc:呼叫物料数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "call_material_qty", ColumnDescription = "呼叫物料数量", IsNullable = true, Length = 255)]
        public string CallMaterialQty{ get; set; }

        /// <summary>
        /// Desc:故障记录编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "fault_record_code", ColumnDescription = "故障记录编号", IsNullable = true, Length = 50)]
        public string FaultRecordCode{ get; set; }

        /// <summary>
        /// Desc:开始应答
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "answer_start", ColumnDescription = "开始应答", IsNullable = true, Length = 11)]
        public int? AnswerStart{ get; set; }

        /// <summary>
        /// Desc:应答时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "answer_time", ColumnDescription = "应答时间", IsNullable = true, Length = 0)]
        public System.DateTime? AnswerTime{ get; set; }

        /// <summary>
        /// Desc:应答人代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "answer_user_name", ColumnDescription = "应答人代码", IsNullable = true, Length = 50)]
        public string AnswerUserName{ get; set; }

        /// <summary>
        /// Desc:应答人姓名
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "answer_user_fullname", ColumnDescription = "应答人姓名", IsNullable = true, Length = 50)]
        public string AnswerUserFullname{ get; set; }

        /// <summary>
        /// Desc:应答内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "answer_message", ColumnDescription = "应答内容", IsNullable = true, Length = 200)]
        public string AnswerMessage{ get; set; }

        /// <summary>
        /// Desc:呼叫解除
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "relieve_start", ColumnDescription = "呼叫解除", IsNullable = true, Length = 11)]
        public int? RelieveStart{ get; set; }

        /// <summary>
        /// Desc:解除时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "relieve_time", ColumnDescription = "解除时间", IsNullable = true, Length = 0)]
        public System.DateTime? RelieveTime{ get; set; }

        /// <summary>
        /// Desc:解除人代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "relieve_user_name", ColumnDescription = "解除人代码", IsNullable = true, Length = 50)]
        public string RelieveUserName{ get; set; }

        /// <summary>
        /// Desc:解除人姓名
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "relieve_user_fullname", ColumnDescription = "解除人姓名", IsNullable = true, Length = 50)]
        public string RelieveUserFullname{ get; set; }

        /// <summary>
        /// Desc:解除叫内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "relieve_message", ColumnDescription = "解除叫内容", IsNullable = true, Length = 200)]
        public string RelieveMessage{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}