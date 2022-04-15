using System.Runtime.Serialization;

namespace UserCenter.Enums
{
    public class UserCenterError
    {
        #region 权限操作

        public readonly static string EMAIL_ERROR = "EMAIL_ERROR";
        public readonly static string TEL_ERROR = "TEL_ERROR";
        public readonly static string SHIFT_IS_NULL = "SHIFT_IS_NULL";
        public readonly static string WORKSTATION_CODE_IS_NULL = "WORKSTATION_CODE_IS_NULL";
        public readonly static string WORKSTATION_NAME_IS_NULL = "WORKSTATION_NAME_IS_NULL";
        public readonly static string SHIFT_NAME_IS_NULL = "SHIFT_NAME_IS_NULL";
        public readonly static string SHIFT_ALREADY_EXIST = "SHIFT_ALREADY_EXIST";
        public readonly static string SHIFT_DOES_NOT_EXIST = "SHIFT_DOES_NOT_EXIST";
        public readonly static string USER_NOT_IN_WORKSTATION = "USER_NOT_IN_WORKSTATION";
        public readonly static string EXCEL_IS_NULL = "EXCEL_IS_NULL";

        /// <summary>
        /// 没有找到权限
        /// </summary>
        public readonly static string PERMISSION_NOT_FOUND = "PERMISSION_NOT_FOUND";

        /// <summary>
        /// 权限ID列表不能为空
        /// </summary>
        public readonly static string PERMISSION_ID_LIST_IS_EMPTY = "PERMISSION_ID_LIST_IS_EMPTY";

        /// <summary>
        /// 权限列表没有找到
        /// </summary>
        public readonly static string PERMISSION_LIST_NOT_FOUND = "PERMISSION_LIST_NOT_FOUND";

        /// <summary>
        /// 权限已经分配给角色了
        /// </summary>
        public readonly static string PERMISSION_ALREADY_ASSIGN = "PERMISSION_ALREADY_ASSIGN";

        /// <summary>
        /// 子权限不是空
        /// </summary>
        public readonly static string SUB_PERMISSION_NOT_EMPTY = "SUB_PERMISSION_NOT_EMPTY";

        /// <summary>
        /// 没有找到关联的权限
        /// </summary>
        public readonly static string ASSIGN_PERMISSION_NOT_FOUND = "ASSIGN_PERMISSION_NOT_FOUND";

        /// <summary>
        ///产线不存在
        /// </summary>
        public readonly static string LINE_DOES_NOT_EXIST = "LINE_DOES_NOT_EXIST";

        /// <summary>
        ///车间不存在
        /// </summary>
        public readonly static string AREA_OBJECT_NOT_EXIST = "AREA_OBJECT_NOT_EXIST";

        /// <summary>
        /// 请选择产品型号
        /// </summary>
        public readonly static string ENGINE_TYPE_NOT_SELECT = "ENGINE_TYPE_NOT_SELECT";

        /// <summary>
        /// 产品型号{0}不存在
        /// </summary>
        public readonly static string ENGINE_TYPE_SELECT_NOT_EXIST = "ENGINE_TYPE_SELECT_NOT_EXIST";

        /// <summary>
        /// 附件列表不能为空
        /// </summary>
        public readonly static string INFRA_UPLOADFILES_NOT_EXIST = "INFRA_UPLOADFILES_NOT_EXIST";

        public readonly static string BUTTON_NAME_ALREADY_EXIST = "BUTTON_NAME_ALREADY_EXIST";
        public readonly static string USER_IS_REPEAT_EXCEL = "USER_IS_REPEAT_EXCEL";

        #endregion 权限操作
        #region 角色操作

        /// <summary>
        /// 分配权限失败
        /// </summary>
        public readonly static string ASSIGN_PERMISSIO_ERROR = "ASSIGN_PERMISSIO_ERROR";

        /// <summary>
        /// 没有找到关联的用户
        /// </summary>
        public readonly static string ASSIGN_USER_NOT_FOUND = "ASSIGN_USER_NOT_FOUND";

        /// <summary>
        /// 角色列表为空
        /// </summary>
        public readonly static string ROLE_LIST_IS_EMPTY = "ROLE_LIST_IS_EMPTY";

        /// <summary>
        ///
        /// </summary>
        public readonly static string JOBNUMBER_ALREADY_EXIST = "JOBNUMBER_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string NAME_ALREADY_EXIST = "NAME_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string OLD_PASSWORD_ERROR = "OLD_PASSWORD_ERROR";

        /// <summary>
        ///
        /// </summary>
        public readonly static string AREA_NAME_ALREADY_EXIST = "AREA_NAME_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string AREA_CODE_ALREADY_EXIST = "AREA_CODE_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string WORKCENTER_DOES_NOT_EXIST = "WORKCENTER_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string SITE_DOES_NOT_EXIST = "SITE_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string LINE_CODE_IS_NULL = "LINE_CODE_IS_NULL";

        /// <summary>
        ///
        /// </summary>
        public readonly static string LINE_CODE_AND_NAME_NOT_MATCH = "LINE_CODE_AND_NAME_NOT_MATCH";

        /// <summary>
        ///
        /// </summary>
        public readonly static string OPCODE_IS_NULL = "OPCODE_IS_NULL";

        /// <summary>
        ///
        /// </summary>
        public readonly static string OPNAME_IS_NULL = "OPNAME_IS_NULL";

        /// <summary>
        ///
        /// </summary>
        public readonly static string WORKSTATION_ALREADY_EXIST = "WORKSTATION_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string WORKSTATION_DOES_NOT_EXIST = "WORKSTATION_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string WORKSTATION_DETAIL_IS_NOT_EMPTY = "WORKSTATION_DETAIL_IS_NOT_EMPTY";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESIGN_NAME_IS_NULL = "ENGINESIGN_NAME_IS_NULL";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESIGN_CODE_IS_NULL = "ENGINESIGN_CODE_IS_NULL";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESIGN_NAME_ALREAD_EXIST = "ENGINESIGN_NAME_ALREAD_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESIGN_CODE_ALREAD_EXIST = "ENGINESIGN_CODE_ALREAD_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESIGN_TYPE_ALREAD_EXIST = "ENGINESIGN_TYPE_ALREAD_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESIGN_TYPE_IS_NULL = "ENGINESIGN_TYPE_IS_NULL";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINETYPE_IS_NULL = "ENGINETYPE_IS_NULL";

        /// <summary>
        ///
        /// </summary>
        public readonly static string NOT_FOUND_ENGINETYPE = "NOT_FOUND_ENGINETYPE";

        /// <summary>
        ///
        /// </summary>
        public readonly static string RECORD_EXIST = "RECORD_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESUBSERVICE_ALREAD_EXIST = "ENGINESUBSERVICE_ALREAD_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string WORKSTATION_DETAIL_DOES_NOT_EXIST = "WORKSTATION_DETAIL_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string WORKSTATION_NAME_ALREADY_EXIST = "WORKSTATION_NAME_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string WORKSTATION_CODE_ALREADY_EXIST = "WORKSTATION_CODE_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ANDONTYPE_DOES_NOT_EXIST = "ANDONTYPE_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string AREA_ALREADY_EXIST = "AREA_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string AREA_DOES_NOT_EXIST = "AREA_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string LINE_IS_NOT_EMPTY = "LINE_IS_NOT_EMPTY";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENGINESIGN_DOES_NOT_EXIST = "ENGINESIGN_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string BOM_DOES_NOT_EXIST = "BOM_DOES_NOT_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string ENTERPRISE_ALREADY_EXIST = "ENTERPRISE_ALREADY_EXIST";

        /// <summary>
        ///
        /// </summary>
        public readonly static string SITE_IS_NOT_EMPTY = "SITE_IS_NOT_EMPTY";

        public readonly static string FORM_CHECK_FAILED = "FORM_CHECK_FAILED";
        public readonly static string EQUIPMENTCLASS_NOT_FOUND = "EQUIPMENTCLASS_NOT_FOUND";
        public readonly static string EQUIPMENT_NOT_FOUND = "EQUIPMENT_NOT_FOUND";
        public readonly static string LINE_ALREADY_EXIST = "LINE_ALREADY_EXIST";
        public readonly static string SITE_ALREADY_EXIST = "SITE_ALREADY_EXIST";
        public readonly static string ENTERPRISE_DOES_NOT_EXIST = "ENTERPRISE_DOES_NOT_EXIST";
        public readonly static string AREA_IS_NOT_EMPTY = "AREA_IS_NOT_EMPTY";

        public readonly static string LINETYPE_ALREADY_EXIST = "LINETYPE_ALREADY_EXIST";
        public readonly static string LINETYPE_DOES_NOT_EXIST = "LINETYPE_DOES_NOT_EXIST";

        #endregion 角色操作

        #region 组织操作

        /// <summary>
        /// 没有找到父组织
        /// </summary>
        public readonly static string ORGANIZATION_PARENT_NOT_FOUND = "ORGANIZATION_PARENT_NOT_FOUND";

        /// <summary>
        /// 用户id列表不能为空
        /// </summary>
        public readonly static string USER_ID_LIST_IS_NULL = "USER_ID_LIST_IS_NULL";

        /// <summary>
        /// 用户没有找到
        /// </summary>
        public readonly static string USER_NOT_FOUND = "USER_NOT_FOUND";

        #endregion 组织操作

        #region 岗位操作

        /// <summary>
        /// 父岗位不存在
        /// </summary>
        public readonly static string POST_PARENT_NOT_FOUND = "POST_PARENT_NOT_FOUND";

        #endregion 岗位操作

    }
}