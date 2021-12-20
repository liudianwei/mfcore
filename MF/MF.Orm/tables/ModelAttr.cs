using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 模型属性配置表 表
    /// </summary>
    [SugarTable("scada_model_attr", "模型属性配置表")]
    public class ModelAttr : BaseEntity
    {
        public ModelAttr()
        {
        }
        
        /// <summary>
        /// Desc:租户code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户code", IsNullable = true, Length = 64)]
        public string TenantCode{ get; set; }

        /// <summary>
        /// Desc:父级主键
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级主键", IsNullable = false, DefaultValue = "0", Length = 36)]
        public string ParentId{ get; set; }

        /// <summary>
        /// Desc:属性主键
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "attr_id", ColumnDescription = "属性主键", IsNullable = false, DefaultValue = "", Length = 36)]
        public string AttrId{ get; set; }

        /// <summary>
        /// Desc:属性值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "attr_value", ColumnDescription = "属性值", IsNullable = true, Length = 255)]
        public string AttrValue{ get; set; }

    }
}