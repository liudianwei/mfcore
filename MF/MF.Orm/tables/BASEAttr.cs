using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 基础节点属性表 表
    /// </summary>
    [SugarTable("scada_base_attr", "基础节点属性表")]
    public class BASEAttr : BaseEntity
    {
        public BASEAttr()
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
        /// Desc:协议主键
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "type_protocol_id", ColumnDescription = "协议主键", IsNullable = false, DefaultValue = "", Length = 36)]
        public string TypeProtocolId{ get; set; }

        /// <summary>
        /// Desc:属性类型:WorkStation,TagClass,TagNode
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "attr_type", ColumnDescription = "属性类型:WorkStation,TagClass,TagNode", IsNullable = false, DefaultValue = "", Length = 64)]
        public string AttrType{ get; set; }

        /// <summary>
        /// Desc:属性代码
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "attr_code", ColumnDescription = "属性代码", IsNullable = false, DefaultValue = "", Length = 64)]
        public string AttrCode{ get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "order_id", ColumnDescription = "排序", IsNullable = false, DefaultValue = "0", Length = 4)]
        public int OrderId{ get; set; }

        /// <summary>
        /// Desc:属性描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "attr_description", ColumnDescription = "属性描述", IsNullable = true, Length = 255)]
        public string AttrDescription{ get; set; }

        /// <summary>
        /// Desc:正则规范
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "regular", ColumnDescription = "正则规范", IsNullable = true, Length = 64)]
        public string Regular{ get; set; }

    }
}