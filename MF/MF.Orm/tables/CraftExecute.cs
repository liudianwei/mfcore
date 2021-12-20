using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 生产工步执行表 表
    /// </summary>
    [SugarTable("sfc_craft_execute", "生产工步执行表")]
    public class CraftExecute : BaseEntity
    {
        public CraftExecute()
        {
        }
        
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
        /// Desc:机型型号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "机型型号", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:操作者编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operator", ColumnDescription = "操作者编号", IsNullable = true, Length = 255)]
        public string Operator{ get; set; }

        /// <summary>
        /// Desc:工艺号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "craft_num", ColumnDescription = "工艺号", IsNullable = true, Length = 64)]
        public string CraftNum{ get; set; }

        /// <summary>
        /// Desc:工艺版本号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "craft_version", ColumnDescription = "工艺版本号", IsNullable = true, Length = 11)]
        public int? CraftVersion{ get; set; }

        /// <summary>
        /// Desc:工步号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_no", ColumnDescription = "工步号", IsNullable = true, Length = 11)]
        public int? StepNo{ get; set; }

        /// <summary>
        /// Desc:工步内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_name", ColumnDescription = "工步内容", IsNullable = true, Length = 255)]
        public string StepName{ get; set; }

        /// <summary>
        /// Desc:工步状态，0默认1合格，2不合格
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_state", ColumnDescription = "工步状态，0默认1合格，2不合格", IsNullable = true, Length = 11)]
        public int? StepState{ get; set; }

        /// <summary>
        /// Desc:工步完成状态，0默认1完成
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_complete", ColumnDescription = "工步完成状态，0默认1完成", IsNullable = true, Length = 11)]
        public int? StepComplete{ get; set; }

        /// <summary>
        /// Desc:工步返修，0默认1返修
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_repair", ColumnDescription = "工步返修，0默认1返修", IsNullable = true, Length = 11)]
        public int? StepRepair{ get; set; }

        /// <summary>
        /// Desc:操作代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "opreate_type_code", ColumnDescription = "操作代码", IsNullable = true, Length = 64)]
        public string OpreateTypeCode{ get; set; }

        /// <summary>
        /// Desc:操作名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "opreate_type_name", ColumnDescription = "操作名称", IsNullable = true, Length = 64)]
        public string OpreateTypeName{ get; set; }

        /// <summary>
        /// Desc:物料排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_order", ColumnDescription = "物料排序", IsNullable = true, Length = 11)]
        public int? ItemOrder{ get; set; }

        /// <summary>
        /// Desc:物料号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_no", ColumnDescription = "物料号", IsNullable = true, Length = 100)]
        public string ItemNo{ get; set; }

        /// <summary>
        /// Desc:物料名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_name", ColumnDescription = "物料名称", IsNullable = true, Length = 255)]
        public string ItemName{ get; set; }

        /// <summary>
        /// Desc:装配数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "assemble_num", ColumnDescription = "装配数量", IsNullable = true, Length = 11)]
        public int? AssembleNum{ get; set; }

        /// <summary>
        /// Desc:单位
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "uom", ColumnDescription = "单位", IsNullable = true, Length = 64)]
        public string Uom{ get; set; }

        /// <summary>
        /// Desc:物料安全库存
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_safety_stock", ColumnDescription = "物料安全库存", IsNullable = true, Length = 11)]
        public int? MaterialSafetyStock{ get; set; }

        /// <summary>
        /// Desc:物料当前库存
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_current_stock", ColumnDescription = "物料当前库存", IsNullable = true, Length = 11)]
        public int? MaterialCurrentStock{ get; set; }

        /// <summary>
        /// Desc:物料条码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_barcode", ColumnDescription = "物料条码", IsNullable = true, Length = 255)]
        public string MaterialBarcode{ get; set; }

        /// <summary>
        /// Desc:物料批次条码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_lot", ColumnDescription = "物料批次条码", IsNullable = true, Length = 255)]
        public string MaterialLot{ get; set; }

        /// <summary>
        /// Desc:供应商代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "supplier_no", ColumnDescription = "供应商代码", IsNullable = true, Length = 64)]
        public string SupplierNo{ get; set; }

        /// <summary>
        /// Desc:供应商名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "supplier_name", ColumnDescription = "供应商名称", IsNullable = true, Length = 255)]
        public string SupplierName{ get; set; }

        /// <summary>
        /// Desc:是否精准验证  1：是 0 否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "verify_flag", ColumnDescription = "是否精准验证  1：是 0 否", IsNullable = true, Length = 11)]
        public string VerifyFlag{ get; set; }
        /// <summary>
        /// Desc:是否批次验证  1：是 0 否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "lot_flag", ColumnDescription = "是否批次验证  1：是 0 否", IsNullable = true, Length = 11)]
        public string LotFlag { get; set; }
        /// <summary>
        /// Desc:是否精准验证  1：是 0 否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "main_flag", ColumnDescription = "是否主物料  1：是 0 否", IsNullable = true, Length = 11)]
        public string MainFlag { get; set; }

        /// <summary>
        /// Desc:是否小总成
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_assemly", ColumnDescription = "是否小总成", IsNullable = true, Length = 11)]
        public int? IsAssemly{ get; set; }

        /// <summary>
        /// Desc:质量排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "quality_order", ColumnDescription = "质量排序", IsNullable = true, Length = 11)]
        public int? QualityOrder{ get; set; }

        /// <summary>
        /// Desc:质量数据
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "quality_data", ColumnDescription = "质量数据", IsNullable = true, Length = 255)]
        public string QualityData{ get; set; }

        /// <summary>
        /// Desc:工步指导图
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "guide_image", ColumnDescription = "工步指导图", IsNullable = true, Length = 255)]
        public string GuideImage{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}