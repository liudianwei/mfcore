using SqlSugar;

using MF.Orm;

namespace DAL.UserCenter.Entities
{
    /// <summary>
    /// 用来存储配置业务表名、业务名称、主键等 表
    /// </summary>
    [SugarTable("infra_operationlog", "用来存储配置业务表名、业务名称、主键等")]
    public partial class Operationlog : BaseEntity
    {
        public Operationlog()
        {
        }

        /// <summary>
        /// Desc:业务名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "business_name", ColumnDescription = "业务名称", IsNullable = true, Length = 64, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string BusinessName { get; set; }

        /// <summary>
        /// Desc:请求头
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "header", ColumnDescription = "请求头", IsNullable = true, Length = 0, ColumnDataType = "longtext,text", DecimalDigits = 0)]
        public string Header { get; set; }

        /// <summary>
        /// Desc:请求参数
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "params", ColumnDescription = "请求参数", IsNullable = true, Length = 0, ColumnDataType = "text", DecimalDigits = 0)]
        public string Params { get; set; }

        /// <summary>
        /// Desc:请求返回值
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "response", ColumnDescription = "请求返回值", IsNullable = true, Length = 1000, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Response { get; set; }

        /// <summary>
        /// Desc:请求路径
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "url", ColumnDescription = "请求路径", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Url { get; set; }

        /// <summary>
        /// Desc:执行结果 状态 0失败  1成功l
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "result", ColumnDescription = "执行结果 状态 0失败  1成功l", IsNullable = true, ColumnDataType = "int", DecimalDigits = 0)]
        public int? Result { get; set; }

        /// <summary>
        /// Desc:ip地址222.92.130.154
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "ip", ColumnDescription = "ip地址222.92.130.154", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Ip { get; set; }

        /// <summary>
        /// Desc:ip区域,如中国江苏省苏州市 电信
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "ip_region", ColumnDescription = "ip区域,如中国江苏省苏州市 电信", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string IpRegion { get; set; }

        /// <summary>
        /// Desc:消耗时间（单位秒 向上取整）
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "last_time", ColumnDescription = "消耗时间（单位秒 向上取整）", IsNullable = true, ColumnDataType = "int", DecimalDigits = 0)]
        public int? LastTime { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true, Length = 255, ColumnDataType = "varchar", DecimalDigits = 0)]
        public string Remark { get; set; }
    }
}