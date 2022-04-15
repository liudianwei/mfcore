using SqlSugar;

using MF.Orm;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 权限角色关系 表
    /// </summary>
    [SugarTable("uc_role_permission", "权限角色关系")]
    public partial class RolePermission : BaseEntity
    {
        public RolePermission()
        {
        }

        /// <summary>
        /// Desc:权限主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "permission_id", ColumnDescription = "权限主键", IsNullable = true, Length = 36, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string PermissionId { get; set; }

        /// <summary>
        /// Desc:角色主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色主键", IsNullable = true, Length = 36, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string RoleId { get; set; }
    }
}