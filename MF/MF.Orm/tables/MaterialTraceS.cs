using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 物料追溯数据明细分线表 表
    /// </summary>
    [SugarTable("sfc_material_trace_s", "物料追溯数据明细分线表")]
    public class MaterialTraceS : BaseEntity
    {
        public MaterialTraceS()
        {
        }
        
        /// <summary>
        /// Desc:物料索引表主键
        /// Default:
        /// Nullable:False)
        /// </summary>
        [SugarColumn(ColumnName = "trace_id", ColumnDescription = "物料索引表主键", IsNullable = false, DefaultValue = "", Length = 36)]
        public string TraceId{ get; set; }

        /// <summary>
        /// Desc:订单号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单号", IsNullable = true, Length = 64)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:总成唯一编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成唯一编号", IsNullable = true, Length = 255)]
        public string ComponentSn{ get; set; }

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
        /// Desc:工位号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号", IsNullable = true, Length = 50)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:产品代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "product_code", ColumnDescription = "产品代码", IsNullable = true, Length = 50)]
        public string ProductCode{ get; set; }

        /// <summary>
        /// Desc:产品型号代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "产品型号代码", IsNullable = true, Length = 36)]
        public int EngineCode{ get; set; }

        /// <summary>
        /// Desc:产品型号名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号名称", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

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
        /// Desc:物料名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料名称", IsNullable = true, Length = 64)]
        public string MaterialCode{ get; set; }

        /// <summary>
        /// Desc:物料描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_des", ColumnDescription = "物料描述", IsNullable = true, Length = 255)]
        public string MaterialDes{ get; set; }

        /// <summary>
        /// Desc:物料类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_type", ColumnDescription = "物料类型", IsNullable = true, Length = 64)]
        public string ItemType{ get; set; }

        /// <summary>
        /// Desc:零件数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_qty", ColumnDescription = "零件数量", IsNullable = true, Length = 11)]
        public int? ItemQty{ get; set; }

        /// <summary>
        /// Desc:是否主物料 0否 1是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_main_material", ColumnDescription = "是否主物料 0否 1是", IsNullable = true, Length = 10)]
        public string IsMainMaterial{ get; set; }

        /// <summary>
        /// Desc:批次扫描条码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_lot", ColumnDescription = "批次扫描条码", IsNullable = true, Length = 255)]
        public string MaterialLot { get; set; }

        /// <summary>
        /// Desc:精确物料条码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_barcode", ColumnDescription = "精确物料条码", IsNullable = true, Length = 255)]
        public string MaterialBarcode { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}