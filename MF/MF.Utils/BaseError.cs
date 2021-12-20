using System.Runtime.Serialization;

namespace MF.Utils
{
    public static class BaseError
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

        #endregion 通用操作

        public readonly static string BOM_CODE_DOES_NOT_NULL = "BOM_CODE_DOES_NOT_NULL";
        public readonly static string BOM_CODE_ALREADY_EXIST_EXCEL = "BOM_CODE_ALREADY_EXIST_EXCEL";
        public readonly static string USER_IS_REPEAT_EXCEL = "USER_IS_REPEAT_EXCEL";
        public readonly static string ANDONDETAIL_IS_REPEAT_EXCEL = "ANDONDETAIL_IS_REPEAT_EXCEL";
        public readonly static string EXPORT_FILE_EMPTY = "EXPORT_FILE_EMPTY";
        public readonly static string EQUIPMENT_TYPE_IS_NOT_EXIST = "EQUIPMENT_TYPE_IS_NOT_EXIST";
        public readonly static string ANDONTYPE_DOES_NOT_EXIST_EXCEL = "ANDONTYPE_DOES_NOT_EXIST_EXCEL";

        #region 权限操作

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

        #endregion 权限操作

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

        #region 订单相关

        /// <summary>
        /// 订单已下发
        /// </summary>
        public readonly static string ORDER_STATUS_ISSUE = "ORDER_STATUS_ISSUE";

        /// <summary>
        /// 订单无法下移
        /// </summary>
        public readonly static string ORDER_STATUS_DOWN = "ORDER_STATUS_DOWN";

        /// <summary>
        /// 订单无法上移
        /// </summary>
        public readonly static string ORDER_STATUS_UP = "ORDER_STATUS_UP";

        /// <summary>
        /// 下移订单不存在
        /// </summary>
        public readonly static string ORDER_STATUS_DOWN_NO_EXIST = "ORDER_STATUS_DOWN_NO_EXIST";

        /// <summary>
        /// 上移订单不存在
        /// </summary>
        public readonly static string ORDER_STATUS_UP_NO_EXIST = "ORDER_STATUS_UP_NO_EXIST";

        /// <summary>
        /// 下移订单状态不一致
        /// </summary>
        public readonly static string ORDER_STATUS_DOWN_STATUS = "ORDER_STATUS_DOWN_STATUS";

        /// <summary>
        /// 上移订单状态不一致
        /// </summary>
        public readonly static string ORDER_STATUS_UP_STATUS = "ORDER_STATUS_UP_STATUS";

        /// <summary>
        /// 订单状态不能修改
        /// </summary>
        public readonly static string ORDER_STATUS_UPDATE = "ORDER_STATUS_UPDATE";

        /// <summary>
        /// 订单计划开始时间不能大于或等于计划结束时间
        /// </summary>
        public readonly static string ORDER_PLANSTTIME_PLANEDTIME = "ORDER_PLANSTTIME_PLANEDTIME";

        /// <summary>
        /// 班次上班时间不能大于或等于班次下班时间
        /// </summary>
        public readonly static string SHIFT_STARTTIME_ENDTIME = "SHIFT_STARTTIME_ENDTIME";

        /// <summary>
        /// 班次上班时间或下班时间和其他班次有冲突
        /// </summary>
        public readonly static string SHIFT_STARTTIME_ENDTIME_CONFLICT = "SHIFT_STARTTIME_ENDTIME_CONFLICT";

        /// <summary>
        /// 订单计划的订单号已存在
        /// </summary>
        public readonly static string ORDERPLAN_ORDERNUM_ALREADY_EXIST = "ORDERPLAN_ORDERNUM_ALREADY_EXIST";

        /// <summary>
        /// excel导入中存在重复的订单号
        /// </summary>
        public readonly static string PLANORDER_IS_EXIST = "PLANORDER_IS_EXIST";

        /// <summary>
        /// 订单计划的订单号为空
        /// </summary>
        public readonly static string ORDER_ORDERNUM_ISNULL = "ORDER_ORDERNUM_ISNULL";

        /// <summary>
        /// 订单计划的计划开始时间格式不正确
        /// </summary>
        public readonly static string ORDER_STRATTIME_FORMAT_ERROR = "ORDER_STRATTIME_FORMAT_ERROR";

        /// <summary>
        /// 订单计划的计划结束时间格式不正确
        /// </summary>
        public readonly static string ORDER_ENDTIME_FORMAT_ERROR = "ORDER_ENDTIME_FORMAT_ERROR";

        /// <summary>
        /// 订单计划的下发的产线类型不存在
        /// </summary>
        public readonly static string ORDER_SEND_LINETYPE_ERROR = "ORDER_SEND_LINETYPE_ERROR";

        public readonly static string ORDER_ORDERPREFIX_ISNULL = "ORDER_ORDERPREFIX_ISNULL";
        public readonly static string ORDER_ORDERPREFIX_NOSAMNE = "ORDER_ORDERPREFIX_NOSAMNE";

        #endregion 订单相关

        #region 工厂建模相关

        public readonly static string INPUT_FILE_EMPTY = "INPUT_FILE_EMPTY";
        public readonly static string BOM_ALREADY_EXIST = "BOM_ALREADY_EXIST";
        public readonly static string MATERIALCODE_DOES_NOT_NULL = "MATERIALCODE_DOES_NOT_NULL";
        public readonly static string MATERIALQTY_DOES_NOT_NULL = "MATERIALQTY_DOES_NOT_NULL";
        public readonly static string LINE_AND_OP_NOT_MATCH = "LINE_AND_OP_NOT_MATCH";
        public readonly static string ANDON_ALREADY_EXIST = "ANDON_ALREADY_EXIST";
        public readonly static string EQUIPMENT_CODE_ALREADY_EXIST = "EQUIPMENT_CODE_ALREADY_EXIST";
        public readonly static string EQUIPMENT_CODE_IS_NULL = "EQUIPMENT_CODE_IS_NULL";
        public readonly static string EQUIPMENT_NAME_IS_NULL = "EQUIPMENT_NAME_IS_NULL";
        public readonly static string EQUIPMENT_TYPE_IS_NULL = "EQUIPMENT_TYPE_IS_NULL";
        public readonly static string EQUIPMENT_MODEL_IS_NULL = "EQUIPMENT_MODEL_IS_NULL";
        public readonly static string EQUIPMENT_OPNAME_IS_NULL = "EQUIPMENT_OPNAME_IS_NULL";
        public readonly static string EQUIPMENT_OPDESC_IS_NULL = "EQUIPMENT_OPDESC_IS_NULL";
        public readonly static string EQUIPMENT_DO_NOT_EXIST = "EQUIPMENT_DO_NOT_EXIST";
        public readonly static string EMAIL_ERROR = "EMAIL_ERROR";
        public readonly static string TEL_ERROR = "TEL_ERROR";
        public readonly static string DICTITEM_KEY_REPEAT = "DICTITEM_KEY_REPEAT";
        public readonly static string WORKSTATION_CODE_IS_NULL = "WORKSTATION_CODE_IS_NULL";
        public readonly static string WORKSTATION_NAME_IS_NULL = "WORKSTATION_NAME_IS_NULL";
        public readonly static string LINE_NAME_IS_NULL = "LINE_NAME_IS_NULL";
        public readonly static string SHIFT_IS_NULL = "SHIFT_IS_NULL";
        public readonly static string SHIFT_ALREADY_EXIST = "SHIFT_ALREADY_EXIST";
        public readonly static string SHIFT_DOES_NOT_EXIST = "SHIFT_DOES_NOT_EXIST";
        public readonly static string SHIFT_NAME_IS_NULL = "SHIFT_NAME_IS_NULL";
        public readonly static string USER_NOT_IN_WORKSTATION = "USER_NOT_IN_WORKSTATION";
        public readonly static string QUALIFIEDTRACE_NOT_FOUND = "QUALIFIEDTRACE_NOT_FOUND";
        public readonly static string LINE_ONLY_BE_ONE = "LINE_ONLY_BE_ONE";
        public readonly static string EQUIPMENT_NAME_ALREADY_EXIST = "EQUIPMENT_NAME_ALREADY_EXIST";
        public readonly static string EXCEL_IS_NULL = "EXCEL_IS_NULL";
        public readonly static string EQUIPMENT_MODEL_ALREADY_EXIST = "EQUIPMENT_MODEL_ALREADY_EXIST";
        public readonly static string ANDONTYPE_DISTINCT_IN_EXCEL = "ANDONTYPE_DISTINCT_IN_EXCEL";
        public readonly static string STOCK_CAN_NOT_FOUND = "STOCK_CAN_NOT_FOUND";
        public readonly static string INVENTORY_LARGE_THAN_QTY = "QTY_LARGE_THAN_QTY";
        public readonly static string ORDERNUM_IS_NULL = "ORDERNUM_IS_NULL";
        public readonly static string JOBNUM_IS_NULL = "JOBNUM_IS_NULL";
        public readonly static string ORDER_NO_MODIFICATION = "ORDER_NO_MODIFICATION";
        public readonly static string ORDERPLAN_JOBNUM_ALREADY_EXIST = "ORDERPLAN_JOBNUM_ALREADY_EXIST";
        public readonly static string ORDERPLAN_SUBCODE_ALREADY_EXIST = "ORDERPLAN_SUBCODE_ALREADY_EXIST";
        public readonly static string FROMINTERFACE_NOT_ALOW_DELETE = "FROMINTERFACE_NOT_ALOW_DELETE";
        public readonly static string UNKNOWN_OPERATE = "UNKNOWN_OPERATE";
        public readonly static string ORDER_STATUS_NOT_ALLOW_SORT = "ORDER_STATUS_NOT_ALLOW_SORT";
        public readonly static string KEY_REPEAT = "KEY_REPEAT";
        public readonly static string MONITOR_IS_NULL = "MONITOR_IS_NULL";
        public readonly static string ANDON_CALL_ERROR = "ANDON_CALL_ERROR";
        public readonly static string ANDON_ANSWER_ERROR = "ANDON_ANSWERED";
        public readonly static string ANDON_RELIEVE_ERROR = "ANDON_ANSWERED";
        public readonly static string ANDON_PLEASE_CALL = "ANDON_PLEASE_CALL";
        public readonly static string ANDON_PLEASE_RELIEVE = "ANDON_PLEASE_RELIEVE";
        public readonly static string ANDON_ANSWERED = "ANDON_ANSWERED";
        public readonly static string DEACTIVE_STATE = "DEACTIVE_STATE";
        public readonly static string TYPECODE_IS_NULL = "TYPECODE_IS_NULL";
        public readonly static string TYPENAME_IS_NULL = "TYPENAME_IS_NULL";

        public readonly static string REPAIRSTATUS_IS_NULL = "REPAIRSTATUS_IS_NULL";
        public readonly static string REPAIRUSER_IS_NULL = "REPAIRUSER_IS_NULL";
        public readonly static string FAULTSOLUTION_IS_NULL = "FAULTSOLUTION_IS_NULL";
        public readonly static string FAULTDESC_IS_NULL = "FAULTDESC_IS_NULL";
        public readonly static string FAULTCODE_IS_NULL = "FAULTCODE_IS_NULL";
        public readonly static string ADD_FAULTCODE_ERROR = "ADD_FAULTCODE_ERROR";
        public readonly static string FAULTSTARTTIME_IS_NULL = "FAULTSTARTTIME_IS_NULL";
        public readonly static string REPAIR_CODE_IS_NULL = "REPAIR_CODE_IS_NULL";
        public readonly static string IMPACTCONDITION_IS_NULL = "IMPACTCONDITION_IS_NULL";
        public readonly static string EQUIPMENTORGFILE_IS_NULL = "EQUIPMENTORGFILE_IS_NULL";
        public readonly static string PROCESSINFO_IS_NULL = "PROCESSINFO_IS_NULL";
        public readonly static string REASONANALYSISFILE_IS_NULL = "REASONANALYSISFILE_IS_NULL";
        public readonly static string IMAGEANALYSISFILE_IS_NULL = "IMAGEANALYSISFILE_IS_NULL";
        public readonly static string RECURRENCEPREVENTION_IS_NULL = "RECURRENCEPREVENTION_IS_NULL";
        public readonly static string ATTFILE_IS_NULL = "ATTFILE_IS_NULL";
        public readonly static string DYNAMODEL_NOT_FOUND = "DYNAMODEL_NOT_FOUND";
        public readonly static string DYNAMODELITEM_NOT_FOUND = "DYNAMODELITEM_NOT_FOUND";
        public readonly static string UPKEEPCONTENTS_IS_NULL = "UPKEEPCONTENTS_IS_NULL";
        public readonly static string UPKEEPCONTENTS_IS_NULL_EDIT = "UPKEEPCONTENTS_IS_NULL_EDIT";
        public readonly static string UPKEEPCONTENTS_IS_NULL_IMPORT = "UPKEEPCONTENTS_IS_NULL_IMPORT";
        public readonly static string UPKEEPCONTENTS_HAS_REPEAT = "UPKEEPCONTENTS_HAS_REPEAT";
        public readonly static string CPCYCLE_LESS_THAN_ZERO = "CPCYCLE_LESS_THAN_ZERO";
        public readonly static string CPFREQUENCY_LESS_THAN_ZERO = "CPFREQUENCY_LESS_THAN_ZERO";
        public readonly static string ACURACYHOLDTIME_LESS_THAN_ZERO = "ACURACYHOLDTIME_LESS_THAN_ZERO";
        public readonly static string UPKEEPTYPE_HAS_RAPEAT = "UPKEEPTYPE_HAS_RAPEAT";
        public readonly static string CALIBRATIONCONTENTS_IS_NULL = "CALIBRATIONCONTENTS_IS_NULL";
        public readonly static string CONTENTIDS_IS_NULL = "CONTENTIDS_IS_NULL";
        public readonly static string UPKEEPSCHEDULEIDS_IS_NULL = "UPKEEPSCHEDULEIDS_IS_NULL";

        #endregion 工厂建模相关

        #region 设备相关

        /// <summary>
        /// 设备
        /// </summary>
        public readonly static string EQUIPMENT_EQUIPMENTCODE_ISNULL = "EQUIPMENT_EQUIPMENTCODE_ISNULL";

        public readonly static string EQUIPMENT_FAULTCODE_ISNULL = "EQUIPMENT_FAULTCODE_ISNULL";
        public readonly static string EQUIPMENT_FAULTDESC_ISNULL = "EQUIPMENT_FAULTDESC_ISNULL";
        public readonly static string EQUIPMENT_FAULTCODE_EXIST = "EQUIPMENT_FAULTCODE_EXIST";
        public readonly static string CALIBRATIONCONTENT_HAS_REPEAT = "CALIBRATIONCONTENT_HAS_REPEAT";
        public readonly static string CALIBRATIONLEVEL_IS_NULL = "CALIBRATIONLEVEL_IS_NULL";
        public readonly static string UPKEEPTYPE_DOES_NOT_EXIST = "UPKEEPTYPE_DOES_NOT_EXIST";
        public readonly static string ROLE_DOES_NOT_EXIST = "ROLE_DOES_NOT_EXIST";
        public readonly static string PLANTYPECODE_DOES_NOT_EXIST = "PLANTYPECODE_DOES_NOT_EXIST";
        public readonly static string PLANTYPECODE_ISNULL = "PLANTYPECODE_ISNULL";
        public readonly static string ROLE_NAME_IS_NULL = "ROLE_NAME_IS_NULL";

        #endregion 设备相关

        #region 工艺相关

        /// <summary>
        /// 工艺
        /// </summary>
        public readonly static string CRAFT_CRAFTCODE_ALREAD_EXIST = "CRAFT_CRAFTCODE_ALREAD_EXIST";

        public readonly static string CRAFT_CRAFTCODE_ADD = "CRAFT_CRAFTCODE_ADD";
        public readonly static string CRAFT_CRAFTVERSION_ALREAD_EXIST = "CRAFT_CRAFTVERSION_ALREAD_EXIST";
        public readonly static string CRAFT_CRAFTCODE_EDIT = "CRAFT_CRAFTCODE_EDIT";
        public readonly static string CRAFT_CRAFTCODE_DELETE = "CRAFT_CRAFTCODE_DELETE";
        public readonly static string CRAFT_ENGINE_OPNAME_STEP_ALREAD_EXIST = "CRAFT_ENGINE_OPNAME_STEP_ALREAD_EXIST";
        public readonly static string EXCEL_IMPORT_FAILED = "EXCEL_IMPORT_FAILED";
        public readonly static string PARENT_ID_PERMISSION_NOT_EMPTY = "PARENT_ID_PERMISSION_NOT_EMPTY";

        #endregion 工艺相关

        public readonly static string BATCH_STATE_NULL = "BATCH_STATE_NULL";
        public readonly static string BATCH_STATE_DATA_NULL = "BATCH_STATE_DATA_NULL";
        public readonly static string STATE_UPDATE_ERROR = "STATE_UPDATE_ERROR";
        public readonly static string SERIALNO_NOT_EXISTS = "SERIALNO_NOT_EXISTS";
        public readonly static string ANDON_DYNA_MODEL_ITEM_ERROR = "ANDON_DYNA_MODEL_ITEM_ERROR";
        public readonly static string RSTTIME_TIME_OVERLAP = "RSTTIME_TIME_OVERLAP";
        public readonly static string MUSIC_TIME_NOTIN_SHIFT = "MUSIC_TIME_NOTIN_SHIFT";
        public readonly static string MUSIC_STARTTIME_ENDTIME_CONFLICT = "MUSIC_STARTTIME_ENDTIME_CONFLICT";
        public readonly static string NOT_IN_TIME_FORMAT = "NOT_IN_TIME_FORMAT";
        public readonly static string PLANTYPECODE_NOT_EXIST = "PLANTYPECODE_NOT_EXIST";
        public readonly static string OPNAME_ENGINETYPE_VERIFY_EXISTS_EXCEL = "OPNAME_ENGINETYPE_VERIFY_EXISTS_EXCEL";
        public readonly static string OPNAME_ENGINETYPE_VERIFY_EXISTS = "OPNAME_ENGINETYPE_VERIFY_EXISTS";
        public readonly static string VERIFYOP_DOES_NOT_EXIST = "VERIFYOP_DOES_NOT_EXIST";
        public readonly static string PRODUCT_PRODUCTCODE_IS_NULL = "PRODUCT_PRODUCTCODE_IS_NULL";
        public readonly static string UPKEEPPLANS_IS_NULL = "UPKEEPPLANS_IS_NULL";

        public readonly static string UPKEEPPLANS_ALREAD_EXISTS = "UPKEEPPLANS_ALREAD_EXISTS";
        public readonly static string UPKEEPPLANS_ADD_FAILED = "UPKEEPPLANS_ADD_FAILED";
        public readonly static string EQUIPMENT_NAME_IS_NOT_CONTAINS_NULL = "EQUIPMENT_NAME_IS_NOT_CONTAINS_NULL";
        public readonly static string EQUIPMENTCAL_TYPE_IS_NOT_EXIST = "EQUIPMENTCAL_TYPE_IS_NOT_EXIST";
        public readonly static string EQUIPMENT_EQUIPMENTCODE_IS_EXIST = "EQUIPMENT_EQUIPMENTCODE_IS_EXIST";
        public readonly static string EQUIPMENT_EQUIPMENTNAMEC_IS_EXIST = "EQUIPMENT_EQUIPMENTNAMEC_IS_EXIST";
        public readonly static string EQUIPMENT_FAULT_STATUS_ERROR = "EQUIPMENT_FAULT_STATUS_ERROR";

        public readonly static string EXPORTLOG_NOT_ALLOW = "EXPORTLOG_NOT_ALLOW";
        public readonly static string PRODUCT_NOT_EXIST = "PRODUCT_NOT_EXIST";
        public readonly static string BTFUNCTIONCODE_IS_NULL = "BTFUNCTIONCODE_IS_NULL";
    }
}