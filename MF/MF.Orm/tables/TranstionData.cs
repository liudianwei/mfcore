using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 生产过程数据－过站数据保存表 表
    /// </summary>
    [SugarTable("sfc_transtion_data", "生产过程数据－过站数据保存表")]
    public class TranstionData : BaseEntity
    {
        public TranstionData()
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
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:工位名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 64)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:订单号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单号", IsNullable = true, Length = 64)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:机型编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "机型编码", IsNullable = true, Length = 36)]
        public int EngineCode{ get; set; }

        /// <summary>
        /// Desc:机型型号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "机型型号", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:班次name
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_name", ColumnDescription = "班次name", IsNullable = true, Length = 64)]
        public string ShiftName{ get; set; }

        /// <summary>
        /// Desc:班次code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_code", ColumnDescription = "班次code", IsNullable = true, Length = 64)]
        public string ShiftCode{ get; set; }

        /// <summary>
        /// Desc:总成唯一编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "component_sn", ColumnDescription = "总成唯一编号", IsNullable = true, Length = 255)]
        public string ComponentSn{ get; set; }

        /// <summary>
        /// Desc:托盘编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "pallet_code", ColumnDescription = "托盘编号", IsNullable = true, Length = 36)]
        public string PalletCode{ get; set; }

        /// <summary>
        /// Desc:操作时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operator_time", ColumnDescription = "操作时间", IsNullable = true, Length = 0)]
        public System.DateTime? OperatorTime{ get; set; }

        /// <summary>
        /// Desc:操作人员
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "operator", ColumnDescription = "操作人员", IsNullable = true, Length = 64)]
        public string Operator{ get; set; }

        /// <summary>
        /// Desc:到达时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "start_time", ColumnDescription = "到达时间", IsNullable = true, Length = 0)]
        public System.DateTime? StartTime{ get; set; }

        /// <summary>
        /// Desc:离开时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "end_time", ColumnDescription = "离开时间", IsNullable = true, Length = 0)]
        public System.DateTime? EndTime{ get; set; }

        /// <summary>
        /// Desc:在线时间 秒
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "online_time", ColumnDescription = "在线时间 秒", IsNullable = true, Length = 11)]
        public int? OnlineTime{ get; set; }

        /// <summary>
        /// Desc:计划在线时间 秒
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "target_time", ColumnDescription = "计划在线时间 秒", IsNullable = true, Length = 11)]
        public int? TargetTime{ get; set; }

        /// <summary>
        /// Desc:合格标志  1合格  2不合格
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "合格标志  1合格  2不合格", IsNullable = true, Length = 10)]
        public string Status{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}