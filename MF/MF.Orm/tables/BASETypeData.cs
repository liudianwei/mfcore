using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 基础数据类型表 表
    /// </summary>
    [SugarTable("scada_base_type_data", "基础数据类型表")]
    public class BASETypeData : BaseEntity
    {
        public BASETypeData()
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
        /// Desc:数据代码
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "data_code", ColumnDescription = "数据代码", IsNullable = false, DefaultValue = "", Length = 64)]
        public string DataCode{ get; set; }

        /// <summary>
        /// Desc:数据描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "data_description", ColumnDescription = "数据描述", IsNullable = true, Length = 255)]
        public string DataDescription{ get; set; }

        /// <summary>
        /// Desc:数据长度
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "data_length", ColumnDescription = "数据长度", IsNullable = false, DefaultValue = "0", Length = 4)]
        public int DataLength{ get; set; }

        /// <summary>
        /// Desc:是否固定长度，1:固定,0:不固定
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "fixed_length", ColumnDescription = "是否固定长度，1:固定,0:不固定", IsNullable = false, DefaultValue = "1", Length = 4)]
        public int FixedLength{ get; set; }

    }
}