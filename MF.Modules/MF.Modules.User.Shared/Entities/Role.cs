using SqlSugar;

using MF.Orm;
using System.Collections.Generic;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 角色 表
    /// </summary>
    [SugarTable("uc_role", "角色")]
    public partial class Role : BaseEntity
    {
        public Role()
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
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Remark { get; set; }

        [Navigate(typeof(RoleUser), nameof(RoleUser.RoleId), nameof(RoleUser.UserId))]//注意顺序
        public List<User> UserList { get; set; }//只能是null不能赋默认值
    }
}