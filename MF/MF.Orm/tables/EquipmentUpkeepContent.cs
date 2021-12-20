using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备保养基本维护信息 表
    /// </summary>
    [SugarTable("fm_equipment_upkeep_content", "设备保养基本维护信息")]
    public class EquipmentUpkeepContent : BaseEntity
    {
        public EquipmentUpkeepContent()
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
        /// Desc:工位名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 100)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:保养类型编码 专业、日常、润滑等
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upkeep_type_code", ColumnDescription = "保养类型编码 专业、日常、润滑等", IsNullable = true, Length = 64)]
        public string UpkeepTypeCode{ get; set; }

        /// <summary>
        /// Desc:保养类型名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upkeep_type_name", ColumnDescription = "保养类型名称", IsNullable = true, Length = 64)]
        public string UpkeepTypeName{ get; set; }

        /// <summary>
        /// Desc:保养内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "upkeep_content", ColumnDescription = "保养内容", IsNullable = true, Length = 500)]
        public string UpkeepContent{ get; set; }

        /// <summary>
        /// Desc:文件名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", IsNullable = true, Length = 255)]
        public string FileName{ get; set; }

        /// <summary>
        /// Desc:保养部门角色
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "保养部门角色", IsNullable = true, Length = 36)]
        public string RoleId{ get; set; }

        /// <summary>
        /// Desc:保养部门角色
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "role_name", ColumnDescription = "保养部门角色", IsNullable = true, Length = 36)]
        public string RoleName{ get; set; }

        /// <summary>
        /// Desc:保养排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "sort", ColumnDescription = "保养排序", IsNullable = true, Length = 11)]
        public int? Sort{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}