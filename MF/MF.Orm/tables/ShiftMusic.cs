using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 班次管理_音乐代码 表
    /// </summary>
    [SugarTable("fm_shift_music", "班次管理_音乐代码")]
    public class ShiftMusic : BaseEntity
    {
        public ShiftMusic()
        {
        }
        
        /// <summary>
        /// Desc:班次编码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_code", ColumnDescription = "班次编码", IsNullable = true, Length = 100)]
        public string ShiftCode{ get; set; }

        /// <summary>
        /// Desc:班次名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "shift_name", ColumnDescription = "班次名称", IsNullable = true, Length = 100)]
        public string ShiftName{ get; set; }

        /// <summary>
        /// Desc:开始时间 HH:MM:SS
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间 HH:MM:SS", IsNullable = true, Length = 8)]
        public string StartTime{ get; set; }

        /// <summary>
        /// Desc:结束时间 HH:MM:SS
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间 HH:MM:SS", IsNullable = true, Length = 8)]
        public string EndTime{ get; set; }

        /// <summary>
        /// Desc:音乐代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "music_code", ColumnDescription = "音乐代码", IsNullable = true, Length = 10)]
        public string MusicCode{ get; set; }

        /// <summary>
        /// Desc:播放音乐时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "music_play_time", ColumnDescription = "播放音乐时间", IsNullable = true, Length = 11)]
        public int? MusicPlayTime{ get; set; }

        /// <summary>
        /// Desc:是否工作　0否1是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_work", ColumnDescription = "是否工作　0否1是", IsNullable = true, Length = 10)]
        public string IsWork{ get; set; }

        /// <summary>
        /// Desc:是否跨天 ０否1是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_day_span", ColumnDescription = "是否跨天 ０否1是", IsNullable = true, Length = 10)]
        public string IsDaySpan{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remarks", ColumnDescription = "备注", IsNullable = true, Length = 500)]
        public string Remarks{ get; set; }

    }
}