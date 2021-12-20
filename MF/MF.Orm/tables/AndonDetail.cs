using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 安东类型安东明细表 表
    /// </summary>
    [SugarTable("fm_andon_detail", "安东类型安东明细表")]
    public class AndonDetail : BaseEntity
    {
        public AndonDetail()
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
        /// Desc:产线name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线name", IsNullable = true, Length = 64)]
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
        /// Desc:安东明细代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_code", ColumnDescription = "安东明细代码", IsNullable = true, Length = 36)]
        public string AndonCode{ get; set; }

        /// <summary>
        /// Desc:安东明细内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_name", ColumnDescription = "安东明细内容", IsNullable = true, Length = 255)]
        public string AndonName{ get; set; }

        /// <summary>
        /// Desc:前端颜色代码0为正常颜色代码  无色1为呼叫 pink ,2为响应 yellow
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "color_code", ColumnDescription = "前端颜色代码0为正常颜色代码  无色1为呼叫 pink ,2为响应 yellow", IsNullable = true, Length = 10)]
        public string ColorCode{ get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "sort", ColumnDescription = "排序", IsNullable = true, Length = 10)]
        public string Sort{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}