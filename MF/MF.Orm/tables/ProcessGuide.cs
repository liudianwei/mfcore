using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工艺指导 表
    /// </summary>
    [SugarTable("fm_process_guide", "工艺指导")]
    public class ProcessGuide : BaseEntity
    {
        public ProcessGuide()
        {
        }
        
        /// <summary>
        /// Desc:机型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "机型", IsNullable = true, Length = 100)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:工位号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号", IsNullable = true, Length = 100)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 100)]
        public string OpDesc{ get; set; }

    }
}