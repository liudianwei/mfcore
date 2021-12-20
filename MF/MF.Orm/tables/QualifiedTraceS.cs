using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 生产过程数－质量数据追溯表分线表 表
    /// </summary>
    [SugarTable("sfc_qualified_trace_s", "生产过程数－质量数据追溯表分线表")]
    public class QualifiedTraceS : BaseEntity
    {
        public QualifiedTraceS()
        {
        }
        
        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tagid", ColumnDescription = "", IsNullable = true, Length = 36)]
        public string Tagid{ get; set; }

        /// <summary>
        /// Desc:索引表主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "id_tagcf", ColumnDescription = "索引表主键", IsNullable = true, Length = 36)]
        public string IdTagcf{ get; set; }

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
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 64)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:订单表
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单表", IsNullable = true, Length = 64)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:产品型号代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "产品型号代码", IsNullable = true, Length = 36)]
        public int EngineCode{ get; set; }

        /// <summary>
        /// Desc:产品型号名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号名称", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:总成唯一编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成唯一编号", IsNullable = true, Length = 255)]
        public string ComponentSn{ get; set; }

        /// <summary>
        /// Desc:生产日期
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operator_time", ColumnDescription = "生产日期", IsNullable = true, Length = 0)]
        public System.DateTime? OperatorTime{ get; set; }

        /// <summary>
        /// Desc:操作者编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operator", ColumnDescription = "操作者编号", IsNullable = true, Length = 255)]
        public string Operator{ get; set; }

        /// <summary>
        /// Desc:班次code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_code", ColumnDescription = "班次code", IsNullable = true, Length = 64)]
        public string ShiftCode{ get; set; }

        /// <summary>
        /// Desc:班次name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_name", ColumnDescription = "班次name", IsNullable = true, Length = 64)]
        public string ShiftName{ get; set; }

        /// <summary>
        /// Desc:测量值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "measure_value", ColumnDescription = "测量值", IsNullable = true, Length = 64)]
        public string MeasureValue{ get; set; }

        /// <summary>
        /// Desc:测量位置
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "measure_position", ColumnDescription = "测量位置", IsNullable = true, Length = 255)]
        public string MeasurePosition{ get; set; }

        /// <summary>
        /// Desc:测量位置短
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "measure_position_short", ColumnDescription = "测量位置短", IsNullable = true, Length = 50)]
        public string MeasurePositionShort{ get; set; }

        /// <summary>
        /// Desc:测量项
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "measure_item", ColumnDescription = "测量项", IsNullable = true, Length = 255)]
        public string MeasureItem{ get; set; }

        /// <summary>
        /// Desc:理论值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "theory_value", ColumnDescription = "理论值", IsNullable = true, Length = 36)]
        public string TheoryValue{ get; set; }

        /// <summary>
        /// Desc:测量上限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upper_limit", ColumnDescription = "测量上限值", IsNullable = true, Length = 36)]
        public string UpperLimit{ get; set; }

        /// <summary>
        /// Desc:测量下限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "lower_limit", ColumnDescription = "测量下限值", IsNullable = true, Length = 36)]
        public string LowerLimit{ get; set; }

        /// <summary>
        /// Desc:测量单位
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "uom", ColumnDescription = "测量单位", IsNullable = true, Length = 64)]
        public string Uom{ get; set; }

        /// <summary>
        /// Desc:列位置
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "column_location", ColumnDescription = "列位置", IsNullable = true, Length = 10)]
        public string ColumnLocation{ get; set; }

        /// <summary>
        /// Desc:合格标志  1合格  2不合格
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "合格标志  1合格  2不合格", IsNullable = true, Length = 10)]
        public string Status{ get; set; }

        /// <summary>
        /// Desc:mes验证状态 OK NG
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "mes_status", ColumnDescription = "mes验证状态 OK NG", IsNullable = true, Length = 20)]
        public string MesStatus{ get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tag_sequence", ColumnDescription = "排序", IsNullable = true, Length = 11)]
        public int? TagSequence{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}