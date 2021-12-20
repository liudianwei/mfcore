using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 安东明细处理分类表 表
    /// </summary>
    [SugarTable("fm_andon_detail_handle", "安东明细处理分类表")]
    public class AndonDetailHandle : BaseEntity
    {
        public AndonDetailHandle()
        {
        }
        
        /// <summary>
        /// Desc:安东类型,维护到数据字典里
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "andon_type", ColumnDescription = "安东类型,维护到数据字典里", IsNullable = true, Length = 64)]
        public string AndonType{ get; set; }

        /// <summary>
        /// Desc:安东处理明细简称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "handle_abbr", ColumnDescription = "安东处理明细简称", IsNullable = true, Length = 64)]
        public string HandleAbbr{ get; set; }

        /// <summary>
        /// Desc:安东处理明细描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "handle_desc", ColumnDescription = "安东处理明细描述", IsNullable = true, Length = 100)]
        public string HandleDesc{ get; set; }

        /// <summary>
        /// Desc:安东处理明细应答周期
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "answer_time", ColumnDescription = "安东处理明细应答周期", IsNullable = true, Length = 11)]
        public int? AnswerTime{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}