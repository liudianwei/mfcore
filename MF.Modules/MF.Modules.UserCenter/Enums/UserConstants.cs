using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCenter.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public class UserConstants
    {
        // 权限类型：目录 ，菜单，按钮
        public readonly static string PERMISSION_CATALOG = "catalog";
        public readonly static string PERMISSION_MENU = "menu";
        public readonly static string PERMISSION_BUTTON = "button";

        // 登录类型
        public readonly static string LDAP_LOGIN = "ldap";
        //public readonly static string SYSTEM_LOGIN = "system";

        public readonly static string LDAP_CONFIG_NAME = "ldapConfig";
        public readonly static string TENANT_CONFIG = "tenantConfig";

        public readonly static string SYS_USER = "系统用户";
        public readonly static string LADP_USER = "LADP用户";

        // 用户登录
        public readonly static string LOGIN_FAILED = "loginFailed";
        public readonly static string LOGIN_OUT = "loginOut";
        public readonly static string LOGIN_IN = "loginIn";

        //解绑角色
        public readonly static string BIND_ROLE = "物料解绑";


    }
}
