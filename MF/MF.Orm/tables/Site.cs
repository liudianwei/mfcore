using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 站点－工厂 表
    /// </summary>
    [SugarTable("fm_site", "站点－工厂")]
    public class Site : BaseEntity
    {
        public Site()
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
        /// Desc:全称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "full_name", ColumnDescription = "全称", IsNullable = true, Length = 255)]
        public string FullName{ get; set; }

        /// <summary>
        /// Desc:地址
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "address", ColumnDescription = "地址", IsNullable = true, Length = 255)]
        public string Address{ get; set; }

        /// <summary>
        /// Desc:企业主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "enterprise_id", ColumnDescription = "企业主键", IsNullable = true, Length = 36)]
        public string EnterpriseId{ get; set; }

        /// <summary>
        /// Desc:计划日
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plan_day", ColumnDescription = "计划日", IsNullable = true, Length = 11)]
        public int? PlanDay{ get; set; }

        /// <summary>
        /// Desc:发运扣除
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "ship_deduction", ColumnDescription = "发运扣除", IsNullable = true, Length = 4)]
        public byte? ShipDeduction{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}