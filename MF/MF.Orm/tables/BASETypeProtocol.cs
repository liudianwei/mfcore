using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 基础协议表 表
    /// </summary>
    [SugarTable("scada_base_type_protocol", "基础协议表")]
    public class BASETypeProtocol : BaseEntity
    {
        public BASETypeProtocol()
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
        /// Desc:驱动主键
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "type_device_id", ColumnDescription = "驱动主键", IsNullable = false, DefaultValue = "", Length = 36)]
        public string TypeDeviceId{ get; set; }

        /// <summary>
        /// Desc:协议代码
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "protocol_code", ColumnDescription = "协议代码", IsNullable = false, DefaultValue = "", Length = 64)]
        public string ProtocolCode{ get; set; }

        /// <summary>
        /// Desc:协议描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "protocol_description", ColumnDescription = "协议描述", IsNullable = true, Length = 255)]
        public string ProtocolDescription{ get; set; }

    }
}