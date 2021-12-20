using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 生产计划管理－订单总表 表
    /// </summary>
    [SugarTable("aps_order_plan", "生产计划管理－订单总表")]
    public class OrderPlan : BaseEntity
    {
        public OrderPlan()
        {
        }
        
        /// <summary>
        /// Desc:订单编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单编号", IsNullable = true, Length = 64)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:产品型号类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号类型", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:计划开始时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plan_st_time", ColumnDescription = "计划开始时间", IsNullable = true, Length = 0)]
        public System.DateTime? PlanStTime{ get; set; }

        /// <summary>
        /// Desc:计划结束时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plan_ed_time", ColumnDescription = "计划结束时间", IsNullable = true, Length = 0)]
        public System.DateTime? PlanEdTime{ get; set; }

        /// <summary>
        /// Desc:计划数量 默认一台
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plan_count", ColumnDescription = "计划数量 默认一台", IsNullable = true, Length = 11)]
        public int? PlanCount{ get; set; }

        /// <summary>
        /// Desc:订单来源  0外部下发,1本地创建 2 Excel导入 配置到数据字典(展示文本可自定义)
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_source", ColumnDescription = "订单来源  0外部下发,1本地创建 2 Excel导入 配置到数据字典(展示文本可自定义)", IsNullable = true, Length = 10)]
        public string OrderSource{ get; set; }

        /// <summary>
        /// Desc:订单状态：0待下发  1已下发
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_status", ColumnDescription = "订单状态：0待下发  1已下发", IsNullable = true, Length = 10)]
        public string OrderStatus{ get; set; }

        /// <summary>
        /// Desc:版本号 上层同步订单的时候带过来的(冗余字段)
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "bom_version", ColumnDescription = "版本号 上层同步订单的时候带过来的(冗余字段)", IsNullable = true, Length = 64)]
        public string BomVersion{ get; set; }

        /// <summary>
        /// Desc:订单备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "订单备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}