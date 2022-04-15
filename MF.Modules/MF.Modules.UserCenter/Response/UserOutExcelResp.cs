using MF.Utils.Excel;

namespace UserCenter.Response
{
    public class UserOutExcelResp
    {
        [ExcelDescription(Name = "账号")]
        public string Name { get; set; }

        [ExcelDescription(Name = "姓名")]
        public string FullName { get; set; }

        [ExcelDescription(Name = "邮箱")]
        public string Email { get; set; }

        [ExcelDescription(Name = "手机")]
        public string Tel { get; set; }

        [ExcelDescription(Name = "角色")]
        public string RoleName { get; set; }

        [ExcelDescription(Name = "状态")]
        public string State { get; set; }
    }
}