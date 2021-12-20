using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工位－明细表－底层业务使用 表
    /// </summary>
    [SugarTable("fm_workstation_detail", "工位－明细表－底层业务使用")]
    public class WorkstationDetail : BaseEntity
    {
        public WorkstationDetail()
        {
        }
        
        /// <summary>
        /// Desc:工位编码，1010
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_code", ColumnDescription = "工位编码，1010", IsNullable = true, Length = 11)]
        public int? OpCode{ get; set; }

        /// <summary>
        /// Desc:function编号缩略
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "mis_code", ColumnDescription = "function编号缩略", IsNullable = true, Length = 36)]
        public string MisCode{ get; set; }

        /// <summary>
        /// Desc:是否分线  1是  0否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_sub", ColumnDescription = "是否分线  1是  0否", IsNullable = true, Length = 11)]
        public string IsSub { get; set; }

        /// <summary>
        /// Desc:是否保存数据  1是  0否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_measuredata", ColumnDescription = "是否保存数据  1是  0否", IsNullable = true, Length = 10)]
        public string IsMeasuredata{ get; set; }

        /// <summary>
        /// Desc:MES是否监控   1  是  0否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_enabled_mes", ColumnDescription = "MES是否监控   1  是  0否", IsNullable = true, Length = 10)]
        public string IsEnabledMes{ get; set; }

        /// <summary>
        /// Desc:是否允许选择订单   1允许  0 否
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_permission", ColumnDescription = "是否允许选择订单   1允许  0 否", IsNullable = true, Length = 10)]
        public string OrderPermission{ get; set; }

        /// <summary>
        /// Desc:工控机ip
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "ipc_ip", ColumnDescription = "工控机ip", IsNullable = true, Length = 100)]
        public string IpcIp{ get; set; }

        /// <summary>
        /// Desc:工控机端口号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "ipc_port", ColumnDescription = "工控机端口号", IsNullable = true, Length = 11)]
        public int? IpcPort{ get; set; }

        /// <summary>
        /// Desc:服务端端口号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "server_port", ColumnDescription = "服务端端口号", IsNullable = true, Length = 11)]
        public int? ServerPort{ get; set; }

        /// <summary>
        /// Desc:连接通讯服务器成功后的IP
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "server_ip", ColumnDescription = "连接通讯服务器成功后的IP", IsNullable = true, Length = 50)]
        public string ServerIp{ get; set; }

        /// <summary>
        /// Desc:区域监控代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "monitor_code", ColumnDescription = "区域监控代码", IsNullable = true, Length = 11)]
        public int? MonitorCode{ get; set; }

        /// <summary>
        /// Desc:MES工位类型  1 上线  2 下线  0 普通
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_type", ColumnDescription = "MES工位类型  1 上线  2 下线  0 普通", IsNullable = true, Length = 36)]
        public string OpType { get; set; }

        /// <summary>
        /// Desc:线体类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_type", ColumnDescription = "线体类型", IsNullable = true, Length = 10)]
        public string LineType { get; set; }

        /// <summary>
        /// Desc:线体类型名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_type_name", ColumnDescription = "线体类型名称", IsNullable = true, Length = 50)]
        public string LineTypeName { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}