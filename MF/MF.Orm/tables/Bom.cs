using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 物料bom 表
    /// </summary>
    [SugarTable("fm_bom", "物料bom")]
    public class Bom : BaseEntity
    {
        public Bom()
        {
        }
        
        /// <summary>
        /// Desc:产线code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "产线code", IsNullable = true, Length = 64)]
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
        /// Desc:产品型号代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "产品型号代码", IsNullable = true, Length = 11)]
        public int? EngineCode{ get; set; }

        /// <summary>
        /// Desc:产品型号名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号名称", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:物料序号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_sn", ColumnDescription = "物料序号", IsNullable = true, Length = 36)]
        public string MaterialSn{ get; set; }

        /// <summary>
        /// Desc:物料号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料号", IsNullable = true, Length = 36)]
        public string MaterialCode{ get; set; }

        /// <summary>
        /// Desc:物料名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", IsNullable = true, Length = 36)]
        public string MaterialName{ get; set; }

        /// <summary>
        /// Desc:物料描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_des", ColumnDescription = "物料描述", IsNullable = true, Length = 36)]
        public string MaterialDes{ get; set; }

        /// <summary>
        /// Desc:物料类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", IsNullable = true, Length = 255)]
        public string MaterialType{ get; set; }

        /// <summary>
        /// Desc:零件图号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "part_no", ColumnDescription = "零件图号", IsNullable = true, Length = 255)]
        public string PartNo{ get; set; }

        /// <summary>
        /// Desc:零件数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_qty", ColumnDescription = "零件数量", IsNullable = true, Length = 11)]
        public int? MaterialQty{ get; set; }

        /// <summary>
        /// Desc:单位
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "uom", ColumnDescription = "单位", IsNullable = true, Length = 36)]
        public string Uom{ get; set; }

        /// <summary>
        /// Desc:是否主物料  1：是 0 否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "main_flag", ColumnDescription = "是否主物料  1：是 0 否", IsNullable = true, Length = 10)]
        public string MainFlag{ get; set; }

        /// <summary>
        /// Desc:是否精准验证  1：是 0 否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "verify_flag", ColumnDescription = "是否精准验证  1：是 0 否", IsNullable = true, Length = 10)]
        public string VerifyFlag{ get; set; }

        /// <summary>
        /// Desc:数据来源 1：手动新建2：导入
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "source", ColumnDescription = "数据来源 1：手动新建2：导入", IsNullable = true, Length = 255)]
        public string Source{ get; set; }

        /// <summary>
        /// Desc:包装规格
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_spec", ColumnDescription = "包装规格", IsNullable = true, Length = 11)]
        public int? MaterialSpec{ get; set; }

        /// <summary>
        /// Desc:拉动方式
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "pull_type", ColumnDescription = "拉动方式", IsNullable = true, Length = 255)]
        public string PullType{ get; set; }

        /// <summary>
        /// Desc:物料版本
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "version_no", ColumnDescription = "物料版本", IsNullable = true, Length = 50)]
        public string VersionNo{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}