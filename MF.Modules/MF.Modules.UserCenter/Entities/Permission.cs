using SqlSugar;

using MF.Orm;
using System.Collections.Generic;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 权限 表
    /// </summary>
    [SugarTable("uc_permission", "权限")]
    public class Permission : BaseEntity
    {
        public Permission()
        {
        }
        
        /// <summary>
        /// Desc:名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "名称", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Name{ get; set; }

        /// <summary>
        /// Desc:代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "code", ColumnDescription = "代码", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Code{ get; set; }

        /// <summary>
        /// Desc:前台url
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "url", ColumnDescription = "前台url", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Url{ get; set; }

        /// <summary>
        /// Desc:标识
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "perms", ColumnDescription = "标识", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Perms{ get; set; }

        /// <summary>
        /// Desc:类型 0:目录 1:菜单 2:按钮
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "type", ColumnDescription = "类型 0:目录 1:菜单 2:按钮", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Type{ get; set; }

        /// <summary>
        /// Desc:图标
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "icon", ColumnDescription = "图标", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Icon{ get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序", IsNullable = true, ColumnDataType = "int", DecimalDigits = 0)]
        public int? OrderNum{ get; set; }

        /// <summary>
        /// Desc:权限api
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "api_url", ColumnDescription = "权限api", IsNullable = true, Length = 500, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string ApiUrl{ get; set; }

        /// <summary>
        /// Desc:父级id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级id", IsNullable = true, Length = 36, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string ParentId{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Remark{ get; set; }
        /// <summary>
        /// Desc:子权限列表
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<Permission> Child { get; set; }
    }
}