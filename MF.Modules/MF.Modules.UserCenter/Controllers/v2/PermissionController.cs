using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MF.MediatR;
using UserCenter.Commands;
using MF.NetCoreApp;
using UserCenter.Dtos;
using MF.NetCoreApp.Attributes;
using Microsoft.Extensions.Localization;
using MF.Utils;

namespace UserCenter.Controllers.v2
{
    [Route("/rest/usercenter/v{version:apiVersion}/permission")]
    [ApiVersion("2")]
    public class PermissionController : ApiBaseController
    {
        private readonly ILogger<PermissionController> _logger;
        private readonly IBus _bus;

        public PermissionController(ILogger<PermissionController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("添加权限")]
        public async Task<IActionResult> Post(PermissionDto dto)
        {
            var response = await _bus.SendAsync(new CreatePermissionCommand { Dto = dto });
            return Result(response);
        }

        /// <summary>
        /// 更新permission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionLog("更新权限")]
        public async Task<IActionResult> Put(string id, PermissionDto dto)
        {
            UpdatePermissionCommand command = new UpdatePermissionCommand
            {
                Id = id,
                Dto = dto
            };
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 删除permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionLog("删除权限")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _bus.SendAsync(new DeletePermissionCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 根据permissiongroupname查询菜单树
        /// </summary>
        /// <param name="permissionGroupName"></param>
        /// <returns></returns>
        [HttpGet("menutree/{permissionGroupName}")]
        public async Task<IActionResult> FindMenuByPermissionGroupName(string permissionGroupName)
        {
            var response = await _bus.SendAsync(new QueryMenuByPermissionGroupNameCommand() { PermissionGroupName = permissionGroupName });
            return Result(response);
        }

        /// <summary>
        /// 根据permissiongroupname查询按钮
        /// </summary>
        /// <param name="permissionGroupName"></param>
        /// <returns></returns>
        [HttpGet("button/{permissionGroupName}")]
        public async Task<IActionResult> FindButtonByPermissionGroupName(string permissionGroupName)
        {
            var response = await _bus.SendAsync(new QueryButtonByPermissionGroupNameCommand() { PermissionGroupName = permissionGroupName });
            return Result(response);
        }

        /// <summary>
        /// 获取role拥有的permission树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("role/tree")]
        public async Task<IActionResult> FindPermissionTreeByRole()
        {
            var response = await _bus.SendAsync(new QueryPermissionTreeByRoleCommand());
            return Result(response);
        }

        /// <summary>
        /// 获取所有permission树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("alltree")]
        public async Task<IActionResult> FindPermissionTree()
        {
            var response = await _bus.SendAsync(new QueryPermissionTreeCommand());
            return Result(response);
        }

        /// <summary>
        /// 批量添加，删除，修改permission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id}/batch-request"), ActionLog("批量操作权限")]
        public async Task<IActionResult> BatchPermission(string id, BatchPermissionCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 根据permission-id获取permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _bus.SendAsync(new QueryPermissionCommand() { Id = id });
            return Result(response);
        }
    }
}