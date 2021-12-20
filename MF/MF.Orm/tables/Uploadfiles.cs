using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 附件上传 表
    /// </summary>
    [SugarTable("infra_uploadfiles", "附件上传")]
    public class Uploadfiles : BaseEntity
    {
        public Uploadfiles()
        {
        }

        /// <summary>
        /// Desc:其他关联表的主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "bill_id", ColumnDescription = "其他关联表的主键", IsNullable = true, Length = 36)]
        public string BillId { get; set; }

        /// <summary>
        /// Desc:上传文件分类：维护到数据字典
        ///1:工艺图片 2:工艺视频 3:工艺卡片 4:相机工位上传的照片
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "file_class", ColumnDescription = "上传文件分类：维护到数据字典 1:工艺图片 2:工艺视频 3:工艺卡片 4:相机工位上传的照片", IsNullable = true, Length = 36)]
        public string FileClass { get; set; }

        /// <summary>
        /// Desc:文件名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", IsNullable = true, Length = 500)]
        public string FileName { get; set; }

        /// <summary>
        /// Desc:文件存储目录
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "file_url", ColumnDescription = "文件存储目录", IsNullable = true, Length = 500)]
        public string FileUrl { get; set; }

        /// <summary>
        /// Desc:文件大小
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "file_size", ColumnDescription = "文件大小", IsNullable = true, Length = 64)]
        public string FileSize { get; set; }

        /// <summary>
        /// Desc:是否主图(工艺图片的属性) 0否1是
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "is_default_show", ColumnDescription = "是否主图(工艺图片的属性) 0否1是", IsNullable = true, Length = 10)]
        public string IsDefaultShow { get; set; }

    }
}