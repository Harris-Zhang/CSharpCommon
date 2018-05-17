using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZ.Common
{
    /// <summary>
    /// 统一消息响应
    /// 和日志模块对应
    /// </summary>
    public class MessageState
    {
        #region 系统模块

        #region 用户登陆操作提示
        /// <summary>
        /// 用户登陆操作提示
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 1: 操作成功</para>
        /// <para> 2: 用户名或密码错误</para>
        /// <para> 3: 用户已被禁用</para>
        /// </param>
        /// <returns></returns>
        public static string GetLoginState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "用户名或密码错误";//msg_error_Incorrect_username_or_password
                    break;
                case 3:
                    result = "用户已被禁用";//msg_user_has_been_disabled
                    break;
                case 4:
                    result = "登陆超时，请重新登陆";// 
                    break;
                case 5:
                    result = "系统异常，菜单列表加载失败，请联系管理员";// 
                    break;

            }
            return result;
        }
        #endregion

        #region 系统用户操作提示
        /// <summary>
        /// 系统用户操作提示
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 0: 操作失败</para>
        /// <para> 1: 操作成功</para>
        /// <para> 2: 非法操作，用户名不能为空</para>
        /// <para> 3: 新密码不能为空</para>
        /// <para> 4: 旧密码不能已新密码一致</para>
        /// <para> 5: 两次密码不一致</para>
        /// <para> 6: 旧密码不正确</para>
        /// <para> 7: 该用户已存在，请确认</para>
        /// <para> 8: 该用户不存在，请确认</para>
        /// </param>
        /// <returns></returns>
        public static string GetSysUserState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 2:
                    result = "非法操作，用户名不能为空";//msg_Illegal_operation_user_name_canno_be_empty
                    break;
                case 3:
                    result = "新密码不能为空";// msg_new_password_cannot_be_empty
                    break;
                case 4:
                    result = "旧密码不能已新密码一致";// msg_old_password_cannot_be_consistent_with_new_password
                    break;
                case 5:
                    result = "两次密码不一致";// msg_two_passwords_are_inconsistent
                    break;
                case 6:
                    result = "旧密码不正确";// msg_incorrect_old_password
                    break;
                case 7:
                    result = "该用户已存在，请确认";//msg_the_user_already_exists_please_confirm
                    break;
                case 8:
                    result = "该用户不存在，请确认";//msg_the_user_does_not_exist_please_confirm
                    break;
            }
            return result;
        }
        #endregion

        #region 系统异常操作提示
        public static string GetSysExceptionState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;

            }
            return result;
        }
        #endregion

        #region 系统日志操作提示
        public static string GetSysLogState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;

            }
            return result;
        }
        #endregion

        #region 角色操作提示
        /// <summary>
        /// 角色操作提示
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 0: 操作成功</para>
        /// <para> 1: 操作成功</para>
        /// <para> 2: 角色名称重复，请重新填写</para>
        /// <para> 3: 角色删除失败，角色不存在</para>
        /// <para> 4: 角色修改失败，角色不存在</para>
        /// </param>
        /// <returns></returns>
        public static string GetSysRoleState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "角色名称重复，请重新填写";//msg_character_name_is_repeated_please_fill_in
                    break;
                case 3:
                    result = "角色删除失败，角色不存在";// msg_role_deletion_failed_role_does_not_exist
                    break;
                case 4:
                    result = "角色修改失败，角色不存在 ";//msg_role_change_failed_role_does_not_exist
                    break;
            }
            return result;
        }

        #endregion

        #region 系统菜单
        /// <summary>
        /// 系统菜单提示
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 0: 操作成功</para>
        /// <para> 1: 操作成功</para>
        /// <para> 2: 该菜单下有子菜单，请先删除子菜单</para>
        /// <para> 3: 添加操作码失败，操作码名称或代码重复</para>
        /// <para> 4: 操作码删除失败，操作码不存在</para>
        /// </param>
        /// <returns></returns>
        public static string GetSysMenuState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//系统异常，请联系管理员确认
                    break;
                case 0:
                    result = "操作失败";//操作失败
                    break;
                case 1:
                    result = "操作成功";//操作成功
                    break;
                case 2:
                    result = "该菜单下有子菜单，请先删除子菜单 ";//该菜单下有子菜单，请先删除子菜单 
                    break;
                case 3:
                    result = "添加操作码失败，操作码名称或代码重复";//添加操作码失败，操作码名称或代码重复 
                    break;
                case 4:
                    result = "操作码删除失败，操作码不存在";//操作码删除失败，操作码不存在 
                    break;
            }
            return result;
        }

        #endregion

        #region 系统权限
        /// <summary>
        /// 系统权限提示
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 0: 操作成功</para>
        /// <para> 1: 操作成功</para>
        /// </param>
        /// <returns></returns>
        public static string GetSysRightState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//d
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;

            }
            return result;
        }

        #endregion

        #region 部门信息
        /// <summary>
        /// 操作部门提示
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 0: 操作成功</para>
        /// <para> 1: 操作成功</para>
        /// <para> 2: 从ERP更新系统部门成功</para>
        /// <para> 3: 删除失败，该部门不存在，请刷新后再试</para>
        /// <para> 4: 更新失败，该部门不存在，请刷新后再试</para>
        /// <para> 5: 删除失败，该部门下存在子部门，请先删除子部门</para>
        /// <para> 6: 删除失败，顶级菜单不能删除</para>
        /// <para> 7: 从ERP导入部门成功</para>
        /// <para> 8: 更新失败，ERP部门信息禁止手动更新</para>
        /// <para> 9: 删除失败，ERP部门信息禁止手动删除</para>
        /// <para>10: 添加失败，ERP部门禁止手动添加</para>
        /// <para>11: 添加失败，父节点不存在，请刷新后在试</para>
        /// </param>
        /// <returns></returns>
        public static string GetSysDeptState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "从ERP更新系统部门成功";//msg_update_the_system_department_from_ERP_successfully 
                    break;
                case 3:
                    result = "删除失败，该部门不存在，请刷新后再试";//msg_delete_failed_the_sector_does_not_exist_please_refresh_and_try_again 
                    break;
                case 4:
                    result = "更新失败，该部门不存在，请刷新后再试";//msg_update_failed_the_sector_does_not_exist_please_refresh_and_try_again 
                    break;
                case 5:
                    result = "删除失败，该部门下存在子部门，请先删除子部门";//msg_delete_failed_there_are_sub_sectors_under_the_Department_please_delete_the_sub_sector  
                    break;
                case 6:
                    result = "删除失败，顶级菜单不能删除";//msg_delete_failed_top_menu_cannot_be_deleted 
                    break;
                case 7:
                    result = "从ERP导入部门成功";//msg_successful_import_from_ERP 
                    break;
                case 8:
                    result = "更新失败，ERP部门信息禁止手动更新";//msg_update_failed_ERP_department_prohibit_manual_update 
                    break;
                case 9:
                    result = "删除失败，ERP部门信息禁止手动删除";//msg_delete_failed_ERP_department_prohibit_manual_deletion  
                    break;
                case 10:
                    result = "添加失败，ERP部门禁止手动添加";//msg_add_failed_ERP_department_banned_manually_add 
                    break;
                case 11:
                    result = "添加失败，父节点不存在，请刷新后在试";//msg_add_failed_parent_node_does_not_exist_please_refresh_after_trial 
                    break;

            }
            return result;
        }

        #endregion

        #region 定时任务模块
        public static string GetSysTaskStatus(int state)
        {
            string result = string.Empty;
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";
                    break;
                case 0:
                    result = "操作失败";
                    break;
                case 1:
                    result = "操作成功";
                    break;
                case 2:
                    result = "任务不存在";
                    break;
                case 3:
                    result = "";
                    break;
                case 4:
                    result = "";
                    break;
                case 5:
                    result = "";
                    break;
            }
            return result;
        }

        #endregion

        #endregion

        #region 基础设定模块

        #region 条码打印机设定
        /// <summary>
        /// 打印机设定提示
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 0: 操作成功</para>
        /// <para> 1: 操作成功</para> 
        /// </param>
        /// <returns></returns>
        public static string GetPrinterState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
            }
            return result;
        }
        #endregion

        #region 货位管理

        /// <summary>
        /// 货位管理
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetCargoLocatorState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "添加失败，货位编号已存在";
                    break;
                case 3:
                    result = "修改失败，货位编号不存在";
                    break;
                case 4:
                    result = "货位不存在，请检查输入的数据是否正确";
                    break;
                case 5:
                    result = "该货位下已有库存，确认要添加到该货位下？";
                    break;
                case 6:
                    result = "该货位已锁定，不允许出入库操作";
                    break;
                
            }
            return result;
        }
        #endregion

        #region 物料基础资料

        /// <summary>
        /// 物料基础资料
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetItemState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "添加失败，料号已存在";
                    break;
                case 3:
                    result = "修改失败，料号不存在";
                    break;
                case 4:
                    result = "物料基础信息不存在，请确认料号是否正确";
                    break;

            }
            return result;
        }
        #endregion

        #region 配置表Parameter
        /// <summary>
        /// 配置表Parameter
        /// </summary>
        /// <param name="state">
        /// <para>-1: 系统异常，请联系管理员确认</para>
        /// <para> 0: 操作成功</para>
        /// <para> 1: 操作成功</para> 
        /// </param>
        /// <returns></returns>
        public static string GetParameterState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
            }
            return result;
        }
        #endregion

        #endregion

        #region 出入库管理

        #region 出入库任务管理

        public static string GetStockTask(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "任务不存在，请刷新后再尝试";
                    break;
                case 3:
                    result = "任务不是新建状态，不能修改信息";
                    break;
                case 4:
                    result = "任务不是手动创建，不能修改信息";
                    break;
                case 5:
                    result = "物料基础资料不存在，请确认料号是否正确";
                    break;
                case 6:
                    result = "货位不存在，请确认货位是否正确";
                    break;
                case 7:
                    result = "手动添加任务失败";
                    break;
                case 8:
                    result = "库存批次不正确，请确认";
                    break;
                case 9:
                    result = "库存不是在库状态，请确认";
                    break;
                case 10:
                    result = "更新库存状态失败";
                    break;
                case 11:
                    result = "任务不是新建状态,不能强制完成";
                    break;
                case 12:
                    result = "任务不是新建状态,不能回收该任务";
                    break;
                case 13:
                    result = "该货位已锁定，不允许出入库操作";
                    break;
                case 14:
                    result = "输入有误，请确认数量是否正确";
                    break;
                case 15:
                    result = "输入有误，请确认标签条码是否正确";
                    break;
                case 16:
                    result = "输入有误，该标签条码状态不正确";
                    break;
                case 17:
                    result = "任务创建成功，但WCS连接异常，无法下发PLC";
                    break;
            }
            return result;
        }
        #endregion

        #region 库存管理

        public static string GetStockDetail(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "添加库存失败";
                    break;
                case 3:
                    result = "自动添加任务失败";
                    break;
                case 4:
                    result = "输入有误，请确认标签条码是否正确";
                    break;
                case 5:
                    result = "库存出货失败";
                    break;
                case 6:
                    result = "库存不是在库状态，请确认";
                    break;
                case 7:
                    result = "输入有误，请确认料号是否正确";
                    break;
                case 8:
                    result = "输入有误，请确认货位是否正确";
                    break;
                case 9:
                    result = "该货位已锁定，不允许出入库操作";
                    break;
                case 10:
                    result = "输入有误，请确认数量是否正确";
                    break;
                case 11:
                    result = "输入有误，请确认标签条码是否正确";
                    break;
                case 12:
                    result = "输入有误，该标签条码状态不正确";
                    break;
            }
            return result;
        }
        #endregion

        #endregion

        #region 公共模块

        #region 标签列印
        public static string GetLabelState(int state)
        {
            string result = "";
            switch (state)
            {
                case -1:
                    result = "系统异常，请联系管理员确认";//msg_system_exception_please_contact_administrator_to_confirm
                    break;
                case 0:
                    result = "操作失败";//msg_operation_failed
                    break;
                case 1:
                    result = "操作成功";//msg_operation_success
                    break;
                case 2:
                    result = "输入有误，请确认料号是否正确";
                    break;
                case 3:
                    result = "输入有误，请确认数量是否正确";
                    break;
                case 4:
                    result = "输入有误，请确认标签批次是否正确";
                    break;
                case 5:
                    result = "标签列印失败，批次不能为空";
                    break;
                case 6:
                    result = "标签列印失败,标签信息不存在,请确认批次是否正确";
                    break;
                case 7:
                    result = "";
                    break;
                case 8:
                    result = "";
                    break;
                case 9:
                    result = "";
                    break;
                case 10:
                    result = "";
                    break;
            }
            return result;
        }

        #endregion

        #endregion
    }
}
