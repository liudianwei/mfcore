using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 库存表 表
    /// </summary>
    [SugarTable("sfc_stock", "库存表")]
    public class Stock : BaseEntity
    {
        public Stock()
        {
        }
        
        /// <summary>
        /// Desc:所属产线代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "所属产线代码", IsNullable = true, Length = 36)]
        public string LineCode{ get; set; }

        /// <summary>
        /// Desc:产线名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线名称", IsNullable = true, Length = 255)]
        public string LineName{ get; set; }

        /// <summary>
        /// Desc:订单编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单编号", IsNullable = true, Length = 100)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:工位名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 100)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:配送单号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "document_num", ColumnDescription = "配送单号", IsNullable = true, Length = 64)]
        public string DocumentNum{ get; set; }

        /// <summary>
        /// Desc:物料编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编号", IsNullable = true, Length = 64)]
        public string MaterialCode{ get; set; }

        /// <summary>
        /// Desc:物料名称描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_des", ColumnDescription = "物料名称描述", IsNullable = true, Length = 255)]
        public string MaterialDes{ get; set; }

        /// <summary>
        /// Desc:零件图号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "part_no", ColumnDescription = "零件图号", IsNullable = true, Length = 64)]
        public string PartNo{ get; set; }

        /// <summary>
        /// Desc:该字段用于垫片逻辑
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "specs", ColumnDescription = "该字段用于垫片逻辑", IsNullable = true, Length = 64)]
        public string Specs{ get; set; }

        /// <summary>
        /// Desc:零件图号版本号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "part_no_version", ColumnDescription = "零件图号版本号", IsNullable = true, Length = 64)]
        public string PartNoVersion{ get; set; }

        /// <summary>
        /// Desc:接收数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "recive_qty", ColumnDescription = "接收数量", IsNullable = true, Length = 11)]
        public int? ReciveQty{ get; set; }

        /// <summary>
        /// Desc:库存数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "qty", ColumnDescription = "库存数量", IsNullable = true, Length = 11)]
        public int? Qty{ get; set; }

        /// <summary>
        /// Desc:消耗数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "consume_qty", ColumnDescription = "消耗数量", IsNullable = true, Length = 11)]
        public int? ConsumeQty{ get; set; }

        /// <summary>
        /// Desc:零件批号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "lot_no", ColumnDescription = "零件批号", IsNullable = true, Length = 64)]
        public string LotNo{ get; set; }

        /// <summary>
        /// Desc:物料类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", IsNullable = true, Length = 64)]
        public string MaterialType{ get; set; }

        /// <summary>
        /// Desc:冻结物料状态 0未冻结 1已冻结
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_status", ColumnDescription = "冻结物料状态 0未冻结 1已冻结", IsNullable = true, Length = 10)]
        public string MaterialStatus{ get; set; }

        /// <summary>
        /// Desc:卡片条码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "barcode", ColumnDescription = "卡片条码", IsNullable = true, Length = 64)]
        public string Barcode{ get; set; }

        /// <summary>
        /// Desc:卡片条码数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "barcode_qty", ColumnDescription = "卡片条码数量", IsNullable = true, Length = 11)]
        public int? BarcodeQty{ get; set; }

        /// <summary>
        /// Desc:供应商编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商编码", IsNullable = true, Length = 64)]
        public string SupplierCode{ get; set; }

        /// <summary>
        /// Desc:供应商名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "supplier_name", ColumnDescription = "供应商名称", IsNullable = true, Length = 64)]
        public string SupplierName{ get; set; }

        /// <summary>
        /// Desc:上限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upper_limit", ColumnDescription = "上限值", IsNullable = true, Length = 11)]
        public int? UpperLimit{ get; set; }

        /// <summary>
        /// Desc:下限值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "lower_limit", ColumnDescription = "下限值", IsNullable = true, Length = 11)]
        public int? LowerLimit{ get; set; }

        /// <summary>
        /// Desc:是否报警0 否 1是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "Is_alarm", ColumnDescription = "是否报警0 否 1是", IsNullable = true, Length = 10)]
        public string IsAlarm{ get; set; }

        /// <summary>
        /// Desc:0待使用;1使用中;2库存用尽
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "use_status", ColumnDescription = "0待使用;1使用中;2库存用尽", IsNullable = true, Length = 10)]
        public string UseStatus{ get; set; }

        /// <summary>
        /// Desc:盘点时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operate_time", ColumnDescription = "盘点时间", IsNullable = true, Length = 0)]
        public System.DateTime? OperateTime{ get; set; }

    }
}