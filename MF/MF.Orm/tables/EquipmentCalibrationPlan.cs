using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 设备标定计划 表
    /// </summary>
    [SugarTable("fm_equipment_calibration_plan", "设备标定计划")]
    public class EquipmentCalibrationPlan : BaseEntity
    {
        public EquipmentCalibrationPlan()
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
        /// Desc:标定计划编号  自定义规则
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_code", ColumnDescription = "标定计划编号  自定义规则", IsNullable = true, Length = 64)]
        public string CpCode{ get; set; }

        /// <summary>
        /// Desc:标定计划类型 数据字典
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_type", ColumnDescription = "标定计划类型 数据字典", IsNullable = true, Length = 10)]
        public string CpType{ get; set; }

        /// <summary>
        /// Desc:标定周期
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_cycle", ColumnDescription = "标定周期", IsNullable = true, Length = 11)]
        public int? CpCycle{ get; set; }

        /// <summary>
        /// Desc:标定频次
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_frequency", ColumnDescription = "标定频次", IsNullable = true, Length = 11)]
        public int? CpFrequency{ get; set; }

        /// <summary>
        /// Desc:预警件数
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_warning_num", ColumnDescription = "预警件数", IsNullable = true, Length = 11)]
        public int? CpWarningNum{ get; set; }

        /// <summary>
        /// Desc:精度保持 时长(秒)
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "acuracy_hold_time", ColumnDescription = "精度保持 时长(秒)", IsNullable = true, Length = 11)]
        public int? AcuracyHoldTime{ get; set; }

        /// <summary>
        /// Desc:预警时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "warning_time", ColumnDescription = "预警时间", IsNullable = true, Length = 0)]
        public System.DateTime? WarningTime{ get; set; }

        /// <summary>
        /// Desc:上次标定时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "last_cp_time", ColumnDescription = "上次标定时间", IsNullable = true, Length = 0)]
        public System.DateTime? LastCpTime{ get; set; }

        /// <summary>
        /// Desc:标定状态 0待标定 1完成标定 2无需标定
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_status", ColumnDescription = "标定状态 0待标定 1完成标定 2无需标定", IsNullable = true, Length = 10)]
        public string CpStatus{ get; set; }

        /// <summary>
        /// Desc:标定完成时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_finish_time", ColumnDescription = "标定完成时间", IsNullable = true, Length = 0)]
        public System.DateTime? CpFinishTime{ get; set; }

        /// <summary>
        /// Desc:标定人员
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "cp_operator", ColumnDescription = "标定人员", IsNullable = true, Length = 50)]
        public string CpOperator{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255)]
        public string Remark{ get; set; }

    }
}