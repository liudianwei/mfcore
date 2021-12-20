using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 生产监控实时表 表
    /// </summary>
    [SugarTable("sfc_produce_monitor", "生产监控实时表")]
    public class ProduceMonitor : BaseEntity
    {
        public ProduceMonitor()
        {
        }

        /// <summary>
        /// Desc:产线code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "产线code", IsNullable = true, Length = 64)]
        public string LineCode { get; set; }

        /// <summary>
        /// Desc:产线name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线name", IsNullable = true, Length = 64)]
        public string LineName { get; set; }

        /// <summary>
        /// Desc:工位名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 64)]
        public string OpName { get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc { get; set; }

        /// <summary>
        /// Desc:订单表
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单表", IsNullable = true, Length = 64)]
        public string OrderNum { get; set; }

        /// <summary>
        /// Desc:机型编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "机型编码", IsNullable = true, Length = 36)]
        public int EngineCode { get; set; }

        /// <summary>
        /// Desc:机型型号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "机型型号", IsNullable = true, Length = 36)]
        public string EngineType { get; set; }

        /// <summary>
        /// Desc:总成唯一编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成唯一编号", IsNullable = true, Length = 255)]
        public string ComponentSn { get; set; }

        /// <summary>
        /// Desc:操作者编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operator", ColumnDescription = "操作者编号", IsNullable = true, Length = 255)]
        public string Operator { get; set; }

        /// <summary>
        /// Desc:托盘号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "pallet_code", ColumnDescription = "托盘号", IsNullable = true, Length = 64)]
        public string PalletCode { get; set; }

        /// <summary>
        /// Desc:班次code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_code", ColumnDescription = "班次code", IsNullable = true, Length = 64)]
        public string ShiftCode { get; set; }

        /// <summary>
        /// Desc:班次name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_name", ColumnDescription = "班次name", IsNullable = true, Length = 64)]
        public string ShiftName { get; set; }

        /// <summary>
        /// Desc:上一订单
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "last_order_num", ColumnDescription = "上一订单", IsNullable = true, Length = 64)]
        public string LastOrderNum { get; set; }

        /// <summary>
        /// Desc:上一机型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "last_engine_type", ColumnDescription = "上一机型", IsNullable = true, Length = 36)]
        public string LastEngineType { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark { get; set; }
    }
}