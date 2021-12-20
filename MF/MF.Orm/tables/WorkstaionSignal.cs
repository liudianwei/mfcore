using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工位－信号配置 表
    /// </summary>
    [SugarTable("fm_workstaion_signal", "工位－信号配置")]
    public class WorkstaionSignal : BaseEntity
    {
        public WorkstaionSignal()
        {
        }
        
        /// <summary>
        /// Desc:工位号，OP1010
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号，OP1010", IsNullable = true, Length = 36)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述，同步器等件上线
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述，同步器等件上线", IsNullable = true, Length = 255)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:信号code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "signal_code", ColumnDescription = "信号code", IsNullable = true, Length = 50)]
        public string SignalCode{ get; set; }

        /// <summary>
        /// Desc:信号名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "signal_name", ColumnDescription = "信号名称", IsNullable = true, Length = 50)]
        public string SignalName{ get; set; }

        /// <summary>
        /// Desc:展示顺序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "sort", ColumnDescription = "展示顺序", IsNullable = true, Length = 11)]
        public int? Sort{ get; set; }

    }
}