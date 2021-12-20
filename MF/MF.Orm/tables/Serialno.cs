using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 流水策略 表
    /// </summary>
    [SugarTable("infra_serialno", "流水策略")]
    public class Serialno : BaseEntity
    {
        public Serialno()
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
        /// Desc:当前流水号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "current_no", ColumnDescription = "当前流水号", IsNullable = true, Length = 18)]
        public System.Decimal? CurrentNo{ get; set; }

        /// <summary>
        /// Desc:类别
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "category", ColumnDescription = "类别", IsNullable = true, Length = 64)]
        public string Category{ get; set; }

        /// <summary>
        /// Desc:当前流水号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "serialno_pattern", ColumnDescription = "当前流水号", ColumnDataType = "longtext", IsNullable = true,  Length = 0)]
        public string SerialnoPattern{ get; set; }

        /// <summary>
        /// Desc:当前天数
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "current_day", ColumnDescription = "当前天数", IsNullable = true, Length = 0)]
        public System.DateTime? CurrentDay{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}