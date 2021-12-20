using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工艺配方模型 表
    /// </summary>
    [SugarTable("fm_craft_recipe", "工艺配方模型")]
    public class CraftRecipe : BaseEntity
    {
        public CraftRecipe()
        {
        }
        
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
        /// Desc:产品名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "engine_name", ColumnDescription = "产品名称", IsNullable = true, Length = 255)]
        public string EngineName{ get; set; }

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
        /// Desc:工步号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_no", ColumnDescription = "工步号", IsNullable = true, Length = 11)]
        public int? StepNo{ get; set; }

        /// <summary>
        /// Desc:工步描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_description", ColumnDescription = "工步描述", IsNullable = true, Length = 255)]
        public string StepDescription{ get; set; }

        /// <summary>
        /// Desc:工步属性名称 
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_attr_name", ColumnDescription = "工步属性名称 ", IsNullable = true, Length = 255)]
        public string StepAttrName{ get; set; }

        /// <summary>
        /// Desc:工步属性代码（默认0）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_attr_code", ColumnDescription = "工步属性代码（默认0）", IsNullable = true, Length = 11)]
        public int? StepAttrCode{ get; set; }

        /// <summary>
        /// Desc:数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "num", ColumnDescription = "数量", IsNullable = true, Length = 11)]
        public int? Num{ get; set; }

        /// <summary>
        /// Desc:物料扣减数量（默认0）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "deduction_num", ColumnDescription = "物料扣减数量（默认0）", IsNullable = true, Length = 11)]
        public int? DeductionNum{ get; set; }

        /// <summary>
        /// Desc:物料代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料代码", IsNullable = true, Length = 255)]
        public string MaterialCode{ get; set; }

        /// <summary>
        /// Desc:程序id
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "program_id", ColumnDescription = "程序id", IsNullable = true, Length = 11)]
        public int? ProgramId{ get; set; }

        /// <summary>
        /// Desc:套筒号（默认0）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "sleeve_id", ColumnDescription = "套筒号（默认0）", IsNullable = true, Length = 11)]
        public int? SleeveId{ get; set; }

        /// <summary>
        /// Desc:拧紧枪号（默认0）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tighte_no", ColumnDescription = "拧紧枪号（默认0）", IsNullable = true, Length = 11)]
        public int? TighteNo{ get; set; }

        /// <summary>
        /// Desc:条码规则1
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "barcode_rule1", ColumnDescription = "条码规则1", IsNullable = true, Length = 255)]
        public string BarcodeRule1{ get; set; }

        /// <summary>
        /// Desc:启用规则2, 0:否, 1:是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_rule2", ColumnDescription = "启用规则2, 0:否, 1:是", IsNullable = true, Length = 11)]
        public int? IsRule2{ get; set; }

        /// <summary>
        /// Desc:条码规则2
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "barcode_rule2", ColumnDescription = "条码规则2", IsNullable = true, Length = 255)]
        public string BarcodeRule2{ get; set; }

        /// <summary>
        /// Desc:启用规则3, 0:否, 1:是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_rule3", ColumnDescription = "启用规则3, 0:否, 1:是", IsNullable = true, Length = 11)]
        public int? IsRule3{ get; set; }

        /// <summary>
        /// Desc:条码规则3
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "barcode_rule3", ColumnDescription = "条码规则3", IsNullable = true, Length = 255)]
        public string BarcodeRule3{ get; set; }

        /// <summary>
        /// Desc:步序节拍
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "step_beat", ColumnDescription = "步序节拍", IsNullable = true, Length = 11)]
        public int? StepBeat{ get; set; }

        /// <summary>
        /// Desc:返修次数
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "repair_times", ColumnDescription = "返修次数", IsNullable = true, Length = 11)]
        public int? RepairTimes{ get; set; }

        /// <summary>
        /// Desc:是否上传 0:否, 1:是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_upLoad", ColumnDescription = "是否上传 0:否, 1:是", IsNullable = true, Length = 11)]
        public int? IsUpLoad{ get; set; }

        /// <summary>
        /// Desc:上传代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upload_code", ColumnDescription = "上传代码", IsNullable = true, Length = 255)]
        public string UploadCode{ get; set; }

    }
}