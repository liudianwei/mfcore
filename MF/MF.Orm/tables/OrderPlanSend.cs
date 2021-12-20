using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 生产计划管理－订单下发表 表
    /// </summary>
    [SugarTable("aps_order_plan_send", "生产计划管理－订单下发表")]
    public class OrderPlanSend : BaseEntity
    {
        public OrderPlanSend()
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
        /// Desc:产线代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "产线代码", IsNullable = true, Length = 64)]
        public string LineCode{ get; set; }

        /// <summary>
        /// Desc:产线名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线名称", IsNullable = true, Length = 64)]
        public string LineName{ get; set; }

        /// <summary>
        /// Desc:线体类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_type", ColumnDescription = "线体类型", IsNullable = true, Length = 50)]
        public string LineType{ get; set; }

        /// <summary>
        /// Desc:线体名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_type_name", ColumnDescription = "线体名称", IsNullable = true, Length = 50)]
        public string LineTypeName{ get; set; }

        /// <summary>
        /// Desc:计划数量 标准件数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "plan_count", ColumnDescription = "计划数量 标准件数量", IsNullable = true, Length = 11)]
        public int? PlanCount{ get; set; }

        /// <summary>
        /// Desc:产品型号类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号类型", IsNullable = true, Length = 64)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:产品型号名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_name", ColumnDescription = "产品型号名称", IsNullable = true, Length = 100)]
        public string EngineName{ get; set; }

        /// <summary>
        /// Desc:订单排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_sort", ColumnDescription = "订单排序", IsNullable = true, Length = 11)]
        public int? OrderSort{ get; set; }

        /// <summary>
        /// Desc:订单执行状态：0待生产(待上线)  1生产中、2已暂停、3已禁用、4已完工
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "exec_status", ColumnDescription = "订单执行状态：0待生产(待上线)  1生产中、2已暂停、3已禁用、4已完工", IsNullable = true, Length = 10)]
        public string ExecStatus{ get; set; }

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
        /// Desc:操作时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operation_time", ColumnDescription = "操作时间", IsNullable = true, Length = 0)]
        public System.DateTime? OperationTime{ get; set; }

        /// <summary>
        /// Desc:操作人
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operation_user", ColumnDescription = "操作人", IsNullable = true, Length = 36)]
        public string OperationUser{ get; set; }

        /// <summary>
        /// Desc:导入时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "import_time", ColumnDescription = "导入时间", IsNullable = true, Length = 0)]
        public System.DateTime? ImportTime{ get; set; }

        /// <summary>
        /// Desc:完工日期
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "finish_date", ColumnDescription = "完工日期", IsNullable = true, Length = 0)]
        public System.DateTime? FinishDate{ get; set; }

        /// <summary>
        /// Desc:上线时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "online_complete_time", ColumnDescription = "上线时间", IsNullable = true, Length = 0)]
        public System.DateTime? OnlineCompleteTime{ get; set; }

        /// <summary>
        /// Desc:重新上线原因
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "reonline_remark", ColumnDescription = "重新上线原因", IsNullable = true, Length = 255)]
        public string ReonlineRemark{ get; set; }

        /// <summary>
        /// Desc:订单来源  0外部下发,1本地创建 2 Excel导入 配置到数据字典(展示文本可自定义)
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_source", ColumnDescription = "订单来源  0外部下发,1本地创建 2 Excel导入 配置到数据字典(展示文本可自定义)", IsNullable = true, Length = 10)]
        public string OrderSource{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

        /// <summary>
        /// Desc:版本号 上层同步订单的时候带过来的(冗余字段)
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "bom_version", ColumnDescription = "版本号 上层同步订单的时候带过来的(冗余字段)", IsNullable = true, Length = 64)]
        public string BomVersion{ get; set; }

    }
}