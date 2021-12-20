using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 权限组权限关系 表
    /// </summary>
    [SugarTable("uc_permission_permissiongroup", "权限组权限关系")]
    public class PermissionPermissiongroup : BaseEntity
    {
        public PermissionPermissiongroup()
        {
        }
        
        /// <summary>
        /// Desc:权限组主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "permissiongroup_id", ColumnDescription = "权限组主键", IsNullable = true, Length = 36)]
        public string PermissiongroupId{ get; set; }

        /// <summary>
        /// Desc:权限主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "permission_id", ColumnDescription = "权限主键", IsNullable = true, Length = 36)]
        public string PermissionId{ get; set; }

    }
}