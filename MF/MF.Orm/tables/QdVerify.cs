using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 质量数据完整性校验 表
    /// </summary>
    [SugarTable("fm_qd_verify", "质量数据完整性校验")]
    public class QdVerify : BaseEntity
    {
        public QdVerify()
        {
        }
        
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
        /// Desc:工位号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号", IsNullable = true, Length = 64)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:校验工位号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "verify_op_name", ColumnDescription = "校验工位号", IsNullable = true, Length = 64)]
        public string VerifyOpName{ get; set; }

        /// <summary>
        /// Desc:机型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "机型", IsNullable = true, Length = 50)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:校验质量数据组数
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "qd_groups", ColumnDescription = "校验质量数据组数", IsNullable = true, Length = 11)]
        public int? QdGroups{ get; set; }

        /// <summary>
        /// Desc:校验物料数据组数，暂时保留
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "mt_groups", ColumnDescription = "校验物料数据组数，暂时保留", IsNullable = true, Length = 11)]
        public int? MtGroups{ get; set; }

        /// <summary>
        /// Desc:是否扫码转线，扫码校验分线数据  0：否 1：是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_transfer", ColumnDescription = "是否扫码转线，扫码校验分线数据  0：否 1：是", IsNullable = true, DefaultValue = "0", Length = 11)]
        public int? IsTransfer{ get; set; }

    }
}