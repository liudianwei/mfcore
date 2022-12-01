using System.Runtime.Serialization;

namespace MF.Utils
{
    public class BaseSystemError
    {
        #region 通用操作

        /// <summary>
        ///
        /// </summary>
        public readonly static string SUCCEED = "SUCCEED";

        /// <summary>
        ///
        /// </summary>
        public readonly static string FAILED = "FAILED";

        /// <summary>
        ///
        /// </summary>
        public readonly static string OBJECT_DOES_NOT_EXIST = "OBJECT_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string PASSWORD_ERROR = "PASSWORD_ERROR";

        /// <summary>
        /// 用户没有找到
        /// </summary>
        public readonly static string USER_NOT_FOUND = "USER_NOT_FOUND";

        /// <summary>
        ///
        /// </summary>
        public readonly static string USERNAME_OR_PASSWORD_ERROR = "USERNAME_OR_PASSWORD_ERROR";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ID_CANNOT_BE_EMPTY = "ID_CANNOT_BE_EMPTY";

        /// <summary>
        ///
        /// </summary>
        public readonly static string OBJECT_ALREADY_EXIST = "OBJECT_ALREADY_EXIST";

        public readonly static string BASIC_DATA_DOT_EXIST = "BASIC_DATA_DOT_EXIST";

        /// <summary>
        /// 对象已禁用
        /// </summary>
        public readonly static string OBJECT_IS_INACTIVE = "OBJECT_IS_INACTIVE";

        /// <summary>
        /// 父id不能是自己
        /// </summary>
        public readonly static string PARENT_ID_IS_SELF = "PARENT_ID_IS_SELF";

        /// <summary>
        /// 不能将父组织调整为子组织
        /// </summary>
        public readonly static string CAN_NOT_ADD_ORGANIZATION_TO_CHILD = "CAN_NOT_ADD_ORGANIZATION_TO_CHILD";

        /// <summary>
        ///
        /// </summary>
        public readonly static string PARENT_ID_CAN_NOT_EQUAL_ID = "PARENT_ID_CAN_NOT_EQUAL_ID";

        /// <summary>
        /// 父id不能为空
        /// </summary>
        public readonly static string PARENT_ID_IS_NULL = "PARENT_ID_IS_NULL";

        /// <summary>
        /// 父id不能是自己的子项
        /// </summary>
        public readonly static string PARENT_ID_IS_CHILDREN = "PARENT_ID_IS_CHILDREN";

        /// <summary>
        /// 对象已经在使用
        /// </summary>
        public readonly static string OBJECT_IS_USING = "OBJECT_IS_USING";

        /// <summary>
        /// 名称不能为空
        /// </summary>
        public readonly static string NAME_CANNOT_BE_EMPTY = "NAME_CANNOT_BE_EMPTY";

        /// <summary>
        /// 真实姓名不能为空
        /// </summary>
        public readonly static string FULLNAME_CANNOT_BE_EMPTY = "FULLNAME_CANNOT_BE_EMPTY";

        /// <summary>
        /// 未知的服务器错误
        /// </summary>
        public readonly static string UNKNOW_SERVER_ERROR = "UNKNOW_SERVER_ERROR";

        /// <summary>
        /// 未定义的数据
        /// </summary>
        public readonly static string UNDEFINED_DATA = "UNDEFINED_DATA";

        /// <summary>
        /// 参数有误
        /// </summary>
        public readonly static string PARAM_IS_ERROR = "PARAM_IS_ERROR";

        /// <summary>
        /// 对象不能为空
        /// </summary>
        public readonly static string OBJECT_CANNOT_BE_NULL = "OBJECT_CANNOT_BE_NULL";

        /// <summary>
        /// 子级不是空的
        /// </summary>
        public readonly static string CHILDREN_IS_NOT_EMPTY = "CHILDREN_IS_NOT_EMPTY";

        /// <summary>
        /// 子组织不为空
        /// </summary>
        public readonly static string CHILD_ORGANIZATION_DO_NOT_NULL = "CHILD_ORGANIZATION_DO_NOT_NULL";

        /// <summary>
        /// 没有操作权限
        /// </summary>
        public readonly static string DO_NOT_HAVE_PERMISSION = "DO_NOT_HAVE_PERMISSION";

        /// <summary>
        /// 未授权
        /// </summary>
        public readonly static string UNAUTHORIZED = "UNAUTHORIZED";

        /// <summary>
        ///
        /// </summary>
        public readonly static string USER_DEACTIVE = "USER_DEACTIVE";

        /// <summary>
        /// 存在关联关系
        /// </summary>
        public readonly static string RELATION_EXIST = "RELATION_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string DYNAMODEL_MUST_BEEN_ACTIVE = "DYNAMODEL_MUST_BEEN_ACTIVE";

        /// <summary>
        /// 数据列表不能为空
        /// </summary>
        public readonly static string LIST_IS_EMPTY = "LIST_IS_EMPTY";

        /// <summary>
        /// 参数不能为空
        /// </summary>
        public readonly static string PARAM_IS_BLANK = "PARAM_IS_BLANK";

        public readonly static string DEFAULT_SHOW_ALREADY_EXIST = "DEFAULT_SHOW_ALREADY_EXIST";

        /// <summary>
        /// 导入失败
        /// </summary>
        public readonly static string IMPORT_FAILD = "IMPORT_FAILD";

        /// <summary>
        /// 导入表格内容不能为空
        /// </summary>
        public readonly static string IMPORT_LIST_IS_EMPTY = "IMPORT_LIST_IS_EMPTY";

        public readonly static string BATCH_STATE_NULL = "BATCH_STATE_NULL";
        public readonly static string BATCH_STATE_DATA_NULL = "BATCH_STATE_DATA_NULL";
        public readonly static string CATEGORY_LANGUAGE_ALREADY_EXIST = "CATEGORY_LANGUAGE_ALREADY_EXIST";
        public readonly static string NO_DATA_RELEASE = "NO_DATA_RELEASE";
        public readonly static string CONNECTION_FAILED = "Connection Failed";
        public readonly static string STATE_UPDATE_ERROR = "STATE_UPDATE_ERROR";
        public readonly static string DATA_ALREAD_EXISTS = "DATA_ALREAD_EXISTS"; 

        #endregion 通用操作
    }
}