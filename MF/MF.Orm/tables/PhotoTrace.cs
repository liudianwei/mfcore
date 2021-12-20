using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 产品照片追溯 表
    /// </summary>
    [SugarTable("sfc_photo_trace", "产品照片追溯")]
    public class PhotoTrace : BaseEntity
    {
        public PhotoTrace()
        {
        }
        
        /// <summary>
        /// Desc:总成号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成号", IsNullable = true, Length = 100)]
        public string ComponentSn{ get; set; }

        /// <summary>
        /// Desc:工位号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号", IsNullable = true, Length = 100)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 100)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:文件名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", IsNullable = true, Length = 255)]
        public string FileName{ get; set; }

        /// <summary>
        /// Desc:合格状态
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "合格状态", IsNullable = true, Length = 64)]
        public string Status{ get; set; }

    }
}