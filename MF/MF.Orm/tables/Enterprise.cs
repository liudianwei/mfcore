using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 企业 表
    /// </summary>
    [SugarTable("fm_enterprise", "企业")]
    public class Enterprise : BaseEntity
    {
        public Enterprise()
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
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

        /// <summary>
        /// Desc:工作中心主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "workcenter_id", ColumnDescription = "工作中心主键", IsNullable = true, Length = 36)]
        public string WorkcenterId{ get; set; }

    }
}