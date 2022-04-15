using SqlSugar;

using MF.Orm;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 访问日志 表
    /// </summary>
    [SugarTable("uc_accesslog", "访问日志")]
    public partial class Accesslog : BaseEntity
    {
        public Accesslog()
        {
        }

        /// <summary>
        /// Desc:账号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "user_name", ColumnDescription = "账号", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string UserName { get; set; }

        /// <summary>
        /// Desc:登录ip
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "ip", ColumnDescription = "登录ip", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Ip { get; set; }

        /// <summary>
        /// Desc:客户端名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "client_name", ColumnDescription = "客户端名称", IsNullable = true, Length = 500, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string ClientName { get; set; }

        /// <summary>
        /// Desc:操作类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "access_type", ColumnDescription = "操作类型", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string AccessType { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Remark { get; set; }
    }
}