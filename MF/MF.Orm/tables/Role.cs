using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 组织 表
    /// </summary>
    [SugarTable("uc_role", "组织")]
    public class Role : BaseEntity
    {
        public Role()
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
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 64)]
        public string Remark{ get; set; }

    }
}