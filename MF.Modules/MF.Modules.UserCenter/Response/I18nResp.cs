using MF.Utils.Excel;
using MiniExcelLibs.Attributes;

namespace MDCenter.Response.I18n
{
    public class I18nResp
    {
        
        [ExcelDescription(Name = "主键")]
        [ExcelColumnName("主键")]
        public string Id { get; set; }

        [ExcelDescription(Name = "模块code")]
        [ExcelColumnName("模块code")]
        public string Code { get; set; }

        [ExcelDescription(Name = "模块名称")]
        [ExcelColumnName("模块名称")]
        public string Name { get; set; }

        [ExcelDescription(Name = "权限组web/pad/ipc")]
        [ExcelColumnName("权限组web/pad/ipc")]
        public string Category { get; set; }

        [ExcelDescription(Name = "json脚本")]
        [ExcelColumnName("json脚本")]
        public string Context { get; set; }

        [ExcelDescription(Name = "语言类型zh_cn/en_us")]
        [ExcelColumnName("语言类型zh_cn/en_us")]
        public string Language { get; set; }

        [ExcelDescription(Name = "json路径")]
        [ExcelColumnName("json路径")]
        public string Path { get; set; }

        [ExcelDescription(Name = "状态：0启用 1禁用 2删除")]
        [ExcelColumnName("状态：0启用 1禁用 2删除")]
        public string State { get; set; }

        [ExcelDescription(Name = "创建人")]
        [ExcelColumnName("创建人")]
        public string Creator { get; set; }

        [ExcelDescription(Name = "更新人")]
        [ExcelColumnName("更新人")]
        public string Updator { get; set; }

        [ExcelDescription(Name = "创建时间")]
        [ExcelColumnName("创建时间")]
        public System.DateTime CreateTime { get; set; }

        [ExcelDescription(Name = "更新时间")]
        [ExcelColumnName("更新时间")]
        public System.DateTime UpdateTime { get; set; }

        [ExcelDescription(Name = "内部版本 乐观锁 标识字段,同一时间不能同时操作")]
        [ExcelColumnName("内部版本 乐观锁 标识字段,同一时间不能同时操作")]
        public int InnerVersion { get; set; }

    }
}
