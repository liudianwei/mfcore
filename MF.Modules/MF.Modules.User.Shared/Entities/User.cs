using System;

using MF.Orm;

using SqlSugar;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 用户 表
    /// </summary>
    [SugarTable("uc_user", "用户")]
    public partial class User : BaseEntity
    {
        public User()
        {
        }

        /// <summary>
        /// Desc:名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "名称", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Name { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Remark { get; set; }

        /// <summary>
        /// Desc:全称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "full_name", ColumnDescription = "全称", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string FullName { get; set; }

        /// <summary>
        /// Desc:密码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "password", ColumnDescription = "密码", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Password { get; set; }

        /// <summary>
        /// Desc:盐值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "salt", ColumnDescription = "盐值", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Salt { get; set; }

        /// <summary>
        /// Desc:电子邮件
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "email", ColumnDescription = "电子邮件", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Email { get; set; }

        /// <summary>
        /// Desc:电话
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tel", ColumnDescription = "电话", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Tel { get; set; }

        /// <summary>
        /// Desc:主题
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "theme", ColumnDescription = "主题", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Theme { get; set; }

        /// <summary>
        /// Desc:上一次密码修改时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "password_change_time", ColumnDescription = "上一次密码修改时间", IsNullable = true, Length = 0, DecimalDigits = 0)]
        public System.DateTime? PasswordChangeTime { get; set; }

        /// <summary>
        /// Desc:用户类型
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "user_type", ColumnDescription = "用户类型", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string UserType { get; set; }
    }
}