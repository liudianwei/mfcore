using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 生产过程数据-质量数据分线索引表 表
    /// </summary>
    [SugarTable("sfc_qualified_trace_index_s", "生产过程数据-质量数据分线索引表")]
    public class QualifiedTraceIndexS : BaseEntity
    {
        public QualifiedTraceIndexS()
        {
        }
        
        /// <summary>
        /// Desc:
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tagid", ColumnDescription = "", IsNullable = true, Length = 36)]
        public string Tagid{ get; set; }

        /// <summary>
        /// Desc:产线code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "产线code", IsNullable = true, Length = 64)]
        public string LineCode{ get; set; }

        /// <summary>
        /// Desc:产线name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线name", IsNullable = true, Length = 64)]
        public string LineName{ get; set; }

        /// <summary>
        /// Desc:工位名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 64)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:订单号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单号", IsNullable = true, Length = 64)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:机型code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "机型code", IsNullable = true, Length = 36)]
        public int EngineCode{ get; set; }

        /// <summary>
        /// Desc:机型类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "机型类型", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:总成号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成号", IsNullable = true, Length = 255)]
        public string ComponentSn{ get; set; }

        /// <summary>
        /// Desc:班次code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_code", ColumnDescription = "班次code", IsNullable = true, Length = 64)]
        public string ShiftCode{ get; set; }

        /// <summary>
        /// Desc:班次name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_name", ColumnDescription = "班次name", IsNullable = true, Length = 64)]
        public string ShiftName{ get; set; }

        /// <summary>
        /// Desc:托盘号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "pallet_code", ColumnDescription = "托盘号", IsNullable = true, Length = 36)]
        public string PalletCode{ get; set; }

        /// <summary>
        /// Desc:plc合格状态 0不合格 1合格
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plc_status", ColumnDescription = "plc合格状态 0不合格 1合格", IsNullable = true, Length = 10)]
        public string PlcStatus{ get; set; }

        /// <summary>
        /// Desc:mes合格状态 0不合格 1合格
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "mes_status", ColumnDescription = "mes合格状态 0不合格 1合格", IsNullable = true, Length = 10)]
        public string MesStatus{ get; set; }

        /// <summary>
        /// Desc:工位类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_type", ColumnDescription = "工位类型", IsNullable = true, Length = 20)]
        public string OpType{ get; set; }

        ///// <summary>
        ///// Desc:操作者
        ///// Default:NULL
        ///// Nullable:True
        ///// </summary>
        //[SugarColumn(ColumnName = "operator", ColumnDescription = "操作者", IsNullable = true, Length = 255)]
        //public string Operator{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}