using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工位滚动消息维护 表
    /// </summary>
    [SugarTable("fm_workstaion_scroll", "工位滚动消息维护")]
    public class WorkstaionScroll : BaseEntity
    {
        public WorkstaionScroll()
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
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:滚动内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "message", ColumnDescription = "滚动内容", IsNullable = true, Length = 500)]
        public string Message{ get; set; }

    }
}