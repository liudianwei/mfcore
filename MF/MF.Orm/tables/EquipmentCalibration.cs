using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备标定内容维护 表
    /// </summary>
    [SugarTable("fm_equipment_calibration", "设备标定内容维护")]
    public class EquipmentCalibration : BaseEntity
    {
        public EquipmentCalibration()
        {
        }
        
        /// <summary>
        /// Desc:设备编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "equipment_code", ColumnDescription = "设备编码", IsNullable = true, Length = 64)]
        public string EquipmentCode{ get; set; }

        /// <summary>
        /// Desc:设备名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "equipment_name", ColumnDescription = "设备名称", IsNullable = true, Length = 64)]
        public string EquipmentName{ get; set; }

        /// <summary>
        /// Desc:标定内容
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "calibration_content", ColumnDescription = "标定内容", IsNullable = true, Length = 255)]
        public string CalibrationContent{ get; set; }

        /// <summary>
        /// Desc:标定等级
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "calibration_level", ColumnDescription = "标定等级", IsNullable = true, Length = 255)]
        public string CalibrationLevel{ get; set; }

        /// <summary>
        /// Desc:标定推送角色
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "标定推送角色", IsNullable = true, Length = 36)]
        public string RoleId{ get; set; }

        /// <summary>
        /// Desc:标定推送角色
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "role_name", ColumnDescription = "标定推送角色", IsNullable = true, Length = 36)]
        public string RoleName{ get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "sort", ColumnDescription = "排序", IsNullable = true, Length = 11)]
        public int? Sort{ get; set; }

    }
}