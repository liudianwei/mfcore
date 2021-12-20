using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 盘点库存 表
    /// </summary>
    [SugarTable("sfc_take_stock", "盘点库存")]
    public class TakeStock : BaseEntity
    {
        public TakeStock()
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
        /// Desc:总成唯一编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成唯一编号", IsNullable = true, Length = 255)]
        public string ComponentSn{ get; set; }

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
        /// Desc:零件图号版本号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "part_no_version", ColumnDescription = "零件图号版本号", IsNullable = true, Length = 64)]
        public string PartNoVersion{ get; set; }

        /// <summary>
        /// Desc:零件批号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "lot_no", ColumnDescription = "零件批号", IsNullable = true, Length = 64)]
        public string LotNo{ get; set; }

        /// <summary>
        /// Desc:接收数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "recive_qty", ColumnDescription = "接收数量", IsNullable = true, Length = 11)]
        public int? ReciveQty{ get; set; }

        /// <summary>
        /// Desc:盘点前库存
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "before_qty", ColumnDescription = "盘点前库存", IsNullable = true, Length = 11)]
        public int? BeforeQty{ get; set; }

        /// <summary>
        /// Desc:盘点后库存
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "after_qty", ColumnDescription = "盘点后库存", IsNullable = true, Length = 11)]
        public int? AfterQty{ get; set; }

        /// <summary>
        /// Desc:盘点库存差异
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "difference_qty", ColumnDescription = "盘点库存差异", IsNullable = true, Length = 11)]
        public int? DifferenceQty{ get; set; }

        /// <summary>
        /// Desc:盘点原因
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "reason", ColumnDescription = "盘点原因", IsNullable = true, Length = 300)]
        public string Reason{ get; set; }

        /// <summary>
        /// Desc:消耗数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "consume_qty", ColumnDescription = "消耗数量", IsNullable = true, Length = 11)]
        public int? ConsumeQty{ get; set; }

        /// <summary>
        /// Desc:消耗时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "consume_time", ColumnDescription = "消耗时间", IsNullable = true, Length = 0)]
        public System.DateTime? ConsumeTime{ get; set; }

        /// <summary>
        /// Desc:消耗类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "consume_type", ColumnDescription = "消耗类型", IsNullable = true, Length = 50)]
        public string ConsumeType{ get; set; }

        /// <summary>
        /// Desc:卡片条码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "barcode", ColumnDescription = "卡片条码", IsNullable = true, Length = 64)]
        public string Barcode{ get; set; }

        /// <summary>
        /// Desc:使用状态0待使用;1使用中;2库存用尽
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "use_status", ColumnDescription = "使用状态0待使用;1使用中;2库存用尽", IsNullable = true, Length = 10)]
        public string UseStatus{ get; set; }

    }
}