using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 音乐代码维护 表
    /// </summary>
    [SugarTable("fm_music", "音乐代码维护")]
    public class Music : BaseEntity
    {
        public Music()
        {
        }
        
        /// <summary>
        /// Desc:音乐代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "music_code", ColumnDescription = "音乐代码", IsNullable = true, Length = 50)]
        public string MusicCode{ get; set; }

        /// <summary>
        /// Desc:文件名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", IsNullable = true, Length = 100)]
        public string FileName{ get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remarks", ColumnDescription = "备注", IsNullable = true, Length = 500)]
        public string Remarks{ get; set; }

    }
}