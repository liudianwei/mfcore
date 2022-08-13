using SqlSugar;

using MF.Orm;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 角色用户关系 表
    /// </summary>
    [SugarTable("uc_role_user", "角色用户关系")]
    public partial class RoleUser : BaseEntity
    {
        public RoleUser()
        {
        }

        /// <summary>
        /// Desc:用户主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户主键", IsNullable = true, Length = 36, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string UserId { get; set; }

        /// <summary>
        /// Desc:角色主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色主键", IsNullable = true, Length = 36, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string RoleId { get; set; }
    }
}