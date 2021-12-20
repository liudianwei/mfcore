using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工艺路线明细表模型 表
    /// </summary>
    [SugarTable("fm_craft_route_datail", "工艺路线明细表模型")]
    public class CraftRouteDatail : BaseEntity
    {
        public CraftRouteDatail()
        {
        }
        
        /// <summary>
        /// Desc:工艺路线主表模型 id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "route_id", ColumnDescription = "工艺路线主表模型 id", IsNullable = true, Length = 255)]
        public string RouteId{ get; set; }

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

        /// <summary>
        /// Desc:排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "craft_order", ColumnDescription = "排序", IsNullable = true, Length = 11)]
        public int? CraftOrder{ get; set; }

        /// <summary>
        /// Desc:工位代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_code", ColumnDescription = "工位代码", IsNullable = true, Length = 11)]
        public int? OpCode{ get; set; }

        /// <summary>
        /// Desc:工位号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位号", IsNullable = true, Length = 255)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 255)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:线体类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_type", ColumnDescription = "线体类型", IsNullable = true, Length = 255)]
        public string LineType{ get; set; }

        /// <summary>
        /// Desc:是否跳站,0:否,1:是（默认为0，暂时未使用）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "isover", ColumnDescription = "是否跳站,0:否,1:是（默认为0，暂时未使用）", IsNullable = true, Length = 11)]
        public int? Isover{ get; set; }

    }
}