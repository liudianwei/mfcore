using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MF.MediatR;
using UserCenter.Commands;
using MF.NetCoreApp;
using System.Collections.Generic;
using MF.NetCoreApp.Attributes;
using Microsoft.Extensions.Localization;
using MF.Utils;

namespace UserCenter.Controllers.v2
{
    [Route("/rest/usercenter/v{version:apiVersion}/role")]
    [ApiVersion("2")]
    public class RoleController : ApiBaseController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IBus _bus;

        public RoleController(ILogger<RoleController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 新增role
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("新增角色")]
        public async Task<IActionResult> Post(CreateRoleCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 修改role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionLog("修改角色")]
        public async Task<IActionResult> Put(string id, UpdateRoleCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 删除role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionLog("删除角色")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _bus.SendAsync(new DeleteRoleCommand(new List<string> { id }));
            return Result(response);
        }

        /// <summary>
        /// 批量删除role
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("batch-delete-request"), ActionLog("批量删除角色")]
        public async Task<IActionResult> BatchDelete(List<string> ids)
        {
            var response = await _bus.SendAsync(new DeleteRoleCommand(ids));
            return Result(response);
        }

        /// <summary>
        /// 根据role-id查询所有permission列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/permission")]
        public async Task<IActionResult> FindPermissionByRoleId(string id)
        {
            var response = await _bus.SendAsync(new QueryPermissionsByRoleIdRoleCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// role分页查询
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("search-result")]
        public async Task<IActionResult> SearchResult(QueryPageRoleCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 给角色分配permission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id}/permission"), ActionLog("给角色分配权限")]
        public async Task<IActionResult> AssignPermissionToRole(string id, AssignPermissionToRoleRoleCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id}/user"), ActionLog("给用户分配角色")]
        public async Task<IActionResult> AssignUserToRole(string id, AssignUserToRoleRoleCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 根据role-id获取user列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/user")]
        public async Task<IActionResult> FindUsersByRoleId(string id)
        {
            var response = await _bus.SendAsync(new QueryUsersByRoleIdRoleRoleCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 获取所有role的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            var response = await _bus.SendAsync(new QueryAllRoleCommand());
            return Result(response);
        }

        /// <summary>
        /// 获取所有role树形的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree")]
        public async Task<IActionResult> AllTree()
        {
            var response = await _bus.SendAsync(new QueryAllTreeRoleCommand());
            return Result(response);
        }

        /// <summary>
        /// 根据role-id获取role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _bus.SendAsync(new QueryRoleCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 根据role-name获取role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("name/{name}")]
        public async Task<IActionResult> FindRoleByName(string name)
        {
            var response = await _bus.SendAsync(new QueryByNameRoleCommand() { Name = name });
            return Result(response);
        }

        /// <summary>
        /// 根据用户名称获取用户所有角色
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("user/{userName}")]
        public async Task<IActionResult> FindRoleByUserName(string userName)
        {
            var response = await _bus.SendAsync(new QueryByUserNameRoleCommand() { UserName = userName });
            return Result(response);
        }
    }
}