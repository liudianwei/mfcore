using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 模型表 表
    /// </summary>
    [SugarTable("scada_model", "模型表")]
    public class Model : BaseEntity
    {
        public Model()
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
        /// Desc:属性类型
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "attr_type", ColumnDescription = "属性类型", IsNullable = false, DefaultValue = "0", Length = 36)]
        public string AttrType{ get; set; }

        /// <summary>
        /// Desc:父级主键
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级主键", IsNullable = false, DefaultValue = "0", Length = 36)]
        public string ParentId{ get; set; }

        /// <summary>
        /// Desc:站点唯一标识
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "unique_id", ColumnDescription = "站点唯一标识", IsNullable = false, DefaultValue = "0", Length = 4)]
        public int UniqueId{ get; set; }

        /// <summary>
        /// Desc:协议主键
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "type_protocol_id", ColumnDescription = "协议主键", IsNullable = false, DefaultValue = "0", Length = 36)]
        public string TypeProtocolId{ get; set; }

        /// <summary>
        /// Desc:运行模式,0:禁用,1:生产,2:开发
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "run_model", ColumnDescription = "运行模式,0:禁用,1:生产,2:开发", IsNullable = false, DefaultValue = "0", Length = 4)]
        public int RunModel{ get; set; }

        /// <summary>
        /// Desc:class类型代码
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "class_type_code", ColumnDescription = "class类型代码", IsNullable = false, DefaultValue = "0", Length = 4)]
        public int ClassTypeCode{ get; set; }

        /// <summary>
        /// Desc:tag唯一ID
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "tag_id", ColumnDescription = "tag唯一ID", IsNullable = false, DefaultValue = "0", Length = 4)]
        public int TagId{ get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "order_id", ColumnDescription = "排序", IsNullable = false, DefaultValue = "0", Length = 4)]
        public int OrderId{ get; set; }

    }
}