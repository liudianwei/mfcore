using SqlSugar;

using MF.Orm;

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
    }
}