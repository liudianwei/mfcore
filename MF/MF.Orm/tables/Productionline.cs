using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 产线 表
    /// </summary>
    [SugarTable("fm_productionline", "产线")]
    public class Productionline : BaseEntity
    {
        public Productionline()
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
        /// Desc:区域主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "area_id", ColumnDescription = "区域主键", IsNullable = true, Length = 36)]
        public string AreaId{ get; set; }

        /// <summary>
        /// Desc:产线类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_type", ColumnDescription = "产线类型", IsNullable = true, Length = 64)]
        public string LineType{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

        /// <summary>
        /// Desc:生产权重
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "multiple", ColumnDescription = "生产权重", IsNullable = true, Length = 11)]
        public int? Multiple{ get; set; }

    }
}