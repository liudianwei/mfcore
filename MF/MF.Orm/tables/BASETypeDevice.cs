using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 基础驱动表 表
    /// </summary>
    [SugarTable("scada_base_type_device", "基础驱动表")]
    public class BASETypeDevice : BaseEntity
    {
        public BASETypeDevice()
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
        /// Desc:驱动代码
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "device_code", ColumnDescription = "驱动代码", IsNullable = false, DefaultValue = "", Length = 64)]
        public string DeviceCode{ get; set; }

        /// <summary>
        /// Desc:驱动描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "device_description", ColumnDescription = "驱动描述", IsNullable = true, Length = 255)]
        public string DeviceDescription{ get; set; }

    }
}