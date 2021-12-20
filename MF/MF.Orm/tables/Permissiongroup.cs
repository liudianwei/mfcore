using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 权限组 表
    /// </summary>
    [SugarTable("uc_permissiongroup", "权限组")]
    public class Permissiongroup : BaseEntity
    {
        public Permissiongroup()
        {
        }
        
        /// <summary>
        /// Desc:名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "名称", IsNullable = true, Length = 64)]
        public string Name{ get; set; }

        /// <summary>
        /// Desc:代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "code", ColumnDescription = "代码", IsNullable = true, Length = 64)]
        public string Code{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}