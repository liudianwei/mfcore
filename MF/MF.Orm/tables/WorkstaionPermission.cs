using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工位－菜单权限关系表 表
    /// </summary>
    [SugarTable("fm_workstaion_permission", "工位－菜单权限关系表")]
    public class WorkstaionPermission : BaseEntity
    {
        public WorkstaionPermission()
        {
        }
        
        /// <summary>
        /// Desc:工位号，OP1010
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号，OP1010", IsNullable = true, Length = 36)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:权限id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "permission_id", ColumnDescription = "权限id", IsNullable = true, Length = 36)]
        public string PermissionId{ get; set; }

    }
}