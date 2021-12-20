using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 字典属性 表
    /// </summary>
    [SugarTable("fm_dyna_model_item", "字典属性")]
    public class DynaModelItem : BaseEntity
    {
        public DynaModelItem()
        {
        }
        
        /// <summary>
        /// Desc:代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_key", ColumnDescription = "代码", IsNullable = true, Length = 64)]
        public string ItemKey{ get; set; }

        /// <summary>
        /// Desc:名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_value", ColumnDescription = "名称", IsNullable = true, Length = 64)]
        public string ItemValue{ get; set; }

        /// <summary>
        /// Desc:字典id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "dyna_model_id", ColumnDescription = "字典id", IsNullable = true, Length = 36)]
        public string DynaModelId{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

        /// <summary>
        /// Desc:值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "item_val", ColumnDescription = "值", IsNullable = true, Length = 255)]
        public string ItemVal{ get; set; }

    }
}