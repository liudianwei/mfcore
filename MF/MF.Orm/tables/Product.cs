using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 产品管理 表
    /// </summary>
    [SugarTable("fm_product", "产品管理")]
    public class Product : BaseEntity
    {
        public Product()
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
        /// Desc:机型编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "机型编码", IsNullable = true, Length = 36)]
        public int EngineCode{ get; set; }

        /// <summary>
        /// Desc:机型型号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "机型型号", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:产品编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "product_code", ColumnDescription = "产品编码", IsNullable = true, Length = 64)]
        public string ProductCode{ get; set; }

        /// <summary>
        /// Desc:产品描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "product_desc", ColumnDescription = "产品描述", IsNullable = true, Length = 64)]
        public string ProductDesc{ get; set; }

        /// <summary>
        /// Desc:客户代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户代码", IsNullable = true, Length = 64)]
        public string CustomerCode{ get; set; }

        /// <summary>
        /// Desc:系列号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "series", ColumnDescription = "系列号", IsNullable = true, Length = 64)]
        public string Series{ get; set; }

        /// <summary>
        /// Desc:系列代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "series_code", ColumnDescription = "系列代码", IsNullable = true, Length = 64)]
        public string SeriesCode{ get; set; }

        /// <summary>
        /// Desc:驱动号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "drive_mode", ColumnDescription = "驱动号", IsNullable = true, Length = 64)]
        public string DriveMode{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}