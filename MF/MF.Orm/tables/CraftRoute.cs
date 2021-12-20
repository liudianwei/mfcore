using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工艺路线主表模型 表
    /// </summary>
    [SugarTable("fm_craft_route", "工艺路线主表模型")]
    public class CraftRoute : BaseEntity
    {
        public CraftRoute()
        {
        }
        
        /// <summary>
        /// Desc:工艺号（未使用，统一默认值）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "craft_code", ColumnDescription = "工艺号（未使用，统一默认值）", IsNullable = true, Length = 255)]
        public string CraftCode{ get; set; }

        /// <summary>
        /// Desc:工艺版本（未使用，统一默认值）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "craft_version", ColumnDescription = "工艺版本（未使用，统一默认值）", IsNullable = true, Length = 255)]
        public string CraftVersion{ get; set; }

        /// <summary>
        /// Desc:工艺描述（未使用，统一默认值）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "craft_description", ColumnDescription = "工艺描述（未使用，统一默认值）", IsNullable = true, Length = 255)]
        public string CraftDescription{ get; set; }

        /// <summary>
        /// Desc:产品代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "产品代码", IsNullable = true, Length = 11)]
        public int? EngineCode{ get; set; }

        /// <summary>
        /// Desc:产品型号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号", IsNullable = true, Length = 255)]
        public string EngineType{ get; set; }

    }
}