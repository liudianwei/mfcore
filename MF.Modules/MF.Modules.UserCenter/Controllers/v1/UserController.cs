using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DAL.UserCenter.IRepository;

using MF.Core.Security;
using MF.FluentValidation;
using MF.MediatR;
using MF.NetCoreApp;
using MF.NetCoreApp.Attributes;
using MF.Utils;
using MF.Utils.Excel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;


using ProductCenter.Commands.ProduceMonitor;

using UserCenter.Commands;
using UserCenter.Dtos;
using UserCenter.Enums;
using UserCenter.Response;

namespace UserCenter.Controllers.v1
{
    [Route("/rest/usercenter/v{version:apiVersion}/user")]
    [ApiVersion("1")]
    public class UserController : ApiBaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IBus _bus;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IBus bus, IUserRepository userRepository, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
            _default_localizer = localizer;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取测试token
        /// </summary>
        /// <returns></returns>
        [HttpGet("testToken/{password}")]
        [AllowAnonymous]
        public async Task<IActionResult> TestToken(string password)
        {
            var response = await _bus.SendAsync(new LoginUserCommand() { Name = SystemConstants.superName, Password = SecurityUtil.ToMd5(password).ToLower(), LoginType = "web" });
            return Result(response);
        }

        /// <summary>
        /// user登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="loginType"></param>
        /// <param name="tenantCode"></param>
        /// <returns></returns>
        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string name, string password, string loginType, string tenantCode)
        {
            var response = await _bus.SendAsync(new LoginUserCommand() { Name = name, Password = password, LoginType = loginType });
            return Result(response);
        }

        /// <summary>
        /// user退出
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string name)
        {
            var response = await _bus.SendAsync(new LogoutUserCommand());
            return Result(response);
        }

        /// <summary>
        /// 创建user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("创建用户")]
        public async Task<IActionResult> Post(UserDto dto)
        {
            var response = await _bus.SendAsync(new CreateUserCommand { Dto = dto });
            return Result(response);
        }

        /// <summary>
        /// 更新user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionLog("更新用户")]
        public async Task<IActionResult> Put(string id, UserDto dto)
        {
            dto.Id = id;
            var response = await _bus.SendAsync(new UpdateUserCommand { Dto = dto });
            return Result(response);
        }

        /// <summary>
        /// 根据user id获取用户组list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/usergroup")]
        public async Task<IActionResult> UserGroupsByUserId(string id)
        {
            var response = await _bus.SendAsync(new QueryUserListByIdCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 根据user name获取用户组list
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("name/{name}/usergroup")]
        public async Task<IActionResult> UserGroupByUserName(string name)
        {
            var response = await _bus.SendAsync(new QueryUserListByNameCommand() { Name = name });
            return Result(response);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionLog("删除用户")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _bus.SendAsync(new DeleteUserCommand() { List = new List<string>() { id } });
            return Result(response);
        }

        /// <summary>
        /// 批量删除user
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("batch-delete-request"), ActionLog("批量删除用户")]
        public async Task<IActionResult> BatchDelete(List<string> ids)
        {
            var response = await _bus.SendAsync(new DeleteUserCommand() { List = ids });
            return Result(response);
        }

        /// <summary>
        /// user分页查询
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("search-result")]
        public async Task<IActionResult> Page(QueryPageUserCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("resetpassword"), ActionLog("重置用户密码")]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 批量重置用户密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("resetpassword/batch"), ActionLog("批量重置用户密码")]
        public async Task<IActionResult> BatchResetPassword(BatchResetPasswordUserCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 修改user密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("changepassword"), ActionLog("修改用户密码")]
        public async Task<IActionResult> ChangePassword(ChangePasswordUserCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 根据user-id获取user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ById(string id)
        {
            var response = await _bus.SendAsync(new QueryUserCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 根据user-id改变user状态
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost("batch-state-request"), ActionLog("启用禁用用户状态")]
        public async Task<IActionResult> ChangeStatus(UpdateStatusUserCommand cmd)
        {
            var response = await _bus.SendAsync(cmd);
            return Result(response);
        }

        /// <summary>
        /// 根据user-name 获取user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("name/{name}")]
        public async Task<IActionResult> ByName(string name)
        {
            var response = await _bus.SendAsync(new QueryByNameUserCommand() { Name = name });
            return Result(response);
        }

        /// <summary>
        /// 根据user-id列表获取user列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<IActionResult> FindUserByIds(List<string> ids)
        {
            var response = await _bus.SendAsync(new QueryUserByIdsUserCommand() { List = ids });
            return Result(response);
        }

        /// <summary>
        /// 修改自定义图标
        /// </summary>
        /// <returns></returns>
        [HttpPut("favorite"), ActionLog("修改用户自定义图标")]
        public async Task<IActionResult> AssignFavorite(AssignFavoriteUserCommand cmd)
        {
            var response = await _bus.SendAsync(cmd);
            return Result(response);
        }

        /// <summary>
        /// 获得自定义图标
        /// </summary>
        /// <returns></returns>
        [HttpGet("favorite")]
        public async Task<IActionResult> FindFavoriteByUserId()
        {
            var response = await _bus.SendAsync(new QueryFavoriteByUserIdUserCommand());
            return Result(response);
        }

        /// <summary>
        /// 查询所有的user
        /// </summary>
        /// <returns></returns>
        [HttpGet("findAllUser")]
        public async Task<IActionResult> FindAllUser()
        {
            var response = await _bus.SendAsync(new QueryAllUserCommand());
            return Result(response);
        }

        /// <summary>
        /// 导入User
        /// </summary>
        /// <returns></returns>
        [HttpPost("import")]
        [AllowAnonymous]
        public async Task<IActionResult> Import()
        {
            var files = Request.Form.Files[0];
            if (files.Length == 0)
            {
                throw new Exception("请选择一个Excel");
            }

            var fs = files.OpenReadStream();
            var name = files.FileName;

            string fileExt = name[name.LastIndexOf('.')..];

            var dt = NpoiUtil.Importdt(fs, fileExt);

            var response = await _bus.SendAsync(new ImportUserCommand() { dataTable = dt });
            return Result(response);
        }

        /// <summary>
        /// 导出User
        /// </summary>
        /// <returns></returns>
        [HttpPost("export")]
        public async Task<IActionResult> Export(ExportUserCommand cmd)
        {
            var resp = await _bus.SendAsync(cmd);
            return await ExportExcel<UserOutExcelResp>(resp, SystemConstants.excelOutPath);
        }

        /// <summary>
        /// 工位登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("ipclogin")]
        [AllowAnonymous]
        public async Task<IActionResult> IPCLogin(string name, string password, string shiftCode, string shiftName, string opName, string opDesc, string lineCode, string lineName)
        {
            var response = await _bus.SendAsync(new IPCLoginUserCommand()
            {
                Name = name,
                Password = password,
                ShiftCode = shiftCode,
                ShiftName = shiftName,
                LineCode=lineCode,
                OpName = opName,
                OpDesc = opDesc,
                LoginType = LoginTypeEnum.Login.Name
            });

            await _bus.SendAsync(new UpdateProduceMonitorCommand() { UserName = name, OpName = opName, OpDesc = opDesc, ShiftCode = shiftCode, ShiftName = shiftName,LineCode=lineCode,LineName=lineName });

            return Result(response);
        }

        /// <summary>
        /// 解绑权限校验
        /// </summary>
        /// <returns></returns>
        [HttpGet("ipcCheckBind"), ActionLog("【IPC】一键解绑校验")]
        public async Task<IActionResult> checkBind(string account, string password)
        {
            var response = await _bus.SendAsync(new IPCCheckBindUserCommand() { Name = account, Password = password });
            return Result(response);
        }
        /// <summary>
        /// IPC工位按钮权限校验
        /// </summary>
        /// <returns></returns>
        [HttpPost("ipcCheckBtn"), ActionLog("【IPC】物料重码验证")]
        public async Task<IActionResult> checkBtn(IPCCheckBtnUserCommand cmd)
        {
            PubResponse response = await _bus.SendAsync(cmd);
            if (response.Status == ResultStatusConstants.SUCCESS)
            {
                response = await _bus.SendAsync(new MDCenter.Commands.WorkstaionBtfunction.CheckWorkstaionBtfunctionCommand()
                {
                    BtfunctionCode = cmd.BtfunctionCode,
                    OpName = cmd.OpName
                }
             );
            } 
            return Result(response);
        }
    }
}