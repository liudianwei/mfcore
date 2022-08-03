using SqlSugar;

using MF.Orm;

namespace DAL.MDCenter.Entities
{
    /// <summary>
    /// 多语言配置 表
    /// </summary>
    [SugarTable("uc_i18n", "多语言配置")]
    public class I18n : BaseEntity
    {
        public I18n()
        {
        }
        
        /// <summary>
        /// Desc:模块code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "code", ColumnDescription = "模块code", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Code{ get; set; }

        /// <summary>
        /// Desc:模块名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "模块名称", IsNullable = true, Length = 50, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Name{ get; set; }

        /// <summary>
        /// Desc:权限组web/pad/ipc
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "category", ColumnDescription = "权限组web/pad/ipc", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Category{ get; set; }

        /// <summary>
        /// Desc:语言类型zh_cn/en_us
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "language", ColumnDescription = "语言类型zh_cn/en_us", IsNullable = true, Length = 30, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Language{ get; set; }

        /// <summary>
        /// Desc:json路径
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "path", ColumnDescription = "json路径", IsNullable = true, Length = 100, ColumnDataType = "varchar", DecimalDigits = 0)]

        public string Path{ get; set; }
        /// <summary>
        /// Desc:json路径
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 100, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Remark { get; set; }

    }
}