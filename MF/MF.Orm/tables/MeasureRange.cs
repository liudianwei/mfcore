using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 测量数据范围 表
    /// </summary>
    [SugarTable("fm_measure_range", "测量数据范围")]
    public class MeasureRange : BaseEntity
    {
        public MeasureRange()
        {
        }
        
        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tag_id", ColumnDescription = "", IsNullable = true, Length = 36)]
        public string TagId{ get; set; }

        /// <summary>
        /// Desc:产品型号类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号类型", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

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
        /// Desc:测量位置
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "measure_position", ColumnDescription = "测量位置", IsNullable = true, Length = 200)]
        public string MeasurePosition{ get; set; }

        /// <summary>
        /// Desc:列位置
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "column_location", ColumnDescription = "列位置", IsNullable = true, Length = 200)]
        public string ColumnLocation{ get; set; }

        /// <summary>
        /// Desc:测量项目
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "measure_item", ColumnDescription = "测量项目", IsNullable = true, Length = 100)]
        public string MeasureItem{ get; set; }

        /// <summary>
        /// Desc:顺序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tag_sequence", ColumnDescription = "顺序", IsNullable = true, Length = 11)]
        public int? TagSequence{ get; set; }

        /// <summary>
        /// Desc:测量单位
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "measure_uom", ColumnDescription = "测量单位", IsNullable = true, Length = 36)]
        public string MeasureUom{ get; set; }

        /// <summary>
        /// Desc:理论值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "theory_value", ColumnDescription = "理论值", IsNullable = true, Length = 10, DecimalDigits = 2)]
        public System.Decimal? TheoryValue{ get; set; }

        /// <summary>
        /// Desc:标准上限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upper_limit", ColumnDescription = "标准上限值", IsNullable = true, Length = 10, DecimalDigits = 2)]
        public System.Decimal? UpperLimit{ get; set; }

        /// <summary>
        /// Desc:标准下限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "lower_limit", ColumnDescription = "标准下限值", IsNullable = true, Length = 10, DecimalDigits = 2)]
        public System.Decimal? LowerLimit{ get; set; }

        /// <summary>
        /// Desc:控制上限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "control_upper_limit", ColumnDescription = "控制上限值", IsNullable = true, Length = 10, DecimalDigits = 2)]
        public System.Decimal? ControlUpperLimit{ get; set; }

        /// <summary>
        /// Desc:控制下限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "control_lower_limit", ColumnDescription = "控制下限值", IsNullable = true, Length = 10, DecimalDigits = 2)]
        public System.Decimal? ControlLowerLimit{ get; set; }

        /// <summary>
        /// Desc:操作时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operate_time", ColumnDescription = "操作时间", IsNullable = true, Length = 0)]
        public System.DateTime? OperateTime{ get; set; }

        /// <summary>
        /// Desc:源数据工位，OP1010
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_source", ColumnDescription = "源数据工位，OP1010", IsNullable = true, Length = 36)]
        public string OpSource{ get; set; }

    }
}