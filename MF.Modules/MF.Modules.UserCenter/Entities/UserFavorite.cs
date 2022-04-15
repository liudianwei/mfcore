using SqlSugar;

using MF.Orm;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 自定义按钮 表
    /// </summary>
    [SugarTable("uc_user_favorite", "自定义按钮")]
    public partial class UserFavorite : BaseEntity
    {
        public UserFavorite()
        {
        }

        /// <summary>
        /// Desc:名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "名称", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Name { get; set; }

        /// <summary>
        /// Desc:颜色
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "color", ColumnDescription = "颜色", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Color { get; set; }

        /// <summary>
        /// Desc:url地址
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "url", ColumnDescription = "url地址", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Url { get; set; }

        /// <summary>
        /// Desc:图标
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "icon", ColumnDescription = "图标", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Icon { get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "display_index", ColumnDescription = "排序", IsNullable = true, ColumnDataType = "int", DecimalDigits = 0)]
        public int? DisplayIndex { get; set; }

        /// <summary>
        /// Desc:所有者id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "owner_id", ColumnDescription = "所有者id", IsNullable = true, Length = 36, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string OwnerId { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Remark { get; set; }
    }
}