using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 产品型号维护表 表
    /// </summary>
    [SugarTable("fm_enginesign", "产品型号维护表")]
    public class Enginesign : BaseEntity
    {
        public Enginesign()
        {
        }
        
        /// <summary>
        /// Desc:产品型号代码，与PLC交互
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_code", ColumnDescription = "产品型号代码，与PLC交互", IsNullable = true, Length = 11)]
        public int? EngineCode{ get; set; }

        /// <summary>
        /// Desc:产品型号名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_name", ColumnDescription = "产品型号名称", IsNullable = true, Length = 36)]
        public string EngineName{ get; set; }

        /// <summary>
        /// Desc:产品型号类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_type", ColumnDescription = "产品型号类型", IsNullable = true, Length = 36)]
        public string EngineType{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}