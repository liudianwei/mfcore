using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MF.MediatR;
using UserCenter.Commands;
using MF.NetCoreApp;
using System.Collections.Generic;
using UserCenter.Dtos;
using MF.NetCoreApp.Attributes;
using Microsoft.Extensions.Localization;
using MF.Utils;

namespace UserCenter.Controllers.v1
{
    [Route("/rest/usercenter/v{version:apiVersion}/permissiongroup")]
    [ApiVersion("1")]
    public class PermissiongroupController : ApiBaseController
    {
        private readonly ILogger<PermissiongroupController> _logger;
        private readonly IBus _bus;

        public PermissiongroupController(ILogger<PermissiongroupController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 创建permissionGroup
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("创建权限组")]
        public async Task<IActionResult> Post(PermissionGroupDto dto)
        {
            var response = await _bus.SendAsync(new CreatePermissionGroupCommand() { Dto = dto });
            return Result(response);
        }

        /// <summary>
        /// 更新permissionGroup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionLog("更新权限组")]
        public async Task<IActionResult> Put(string id, UpdatePermissionGroupCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 删除permissionGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionLog("删除权限组")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _bus.SendAsync(new DeletePermissionGroupCommand(new List<string>() { id }));
            return Result(response);
        }

        /// <summary>
        /// 批量删除permissionGroup
        /// </summary>
        /// <returns></returns>
        [HttpPost("batch-delete-request"), ActionLog("批量删除权限组")]
        public async Task<IActionResult> BatchDelete([FromBody] List<string> ids)
        {
            var response = await _bus.SendAsync(new DeletePermissionGroupCommand(ids));
            return Result(response);
        }

        /// <summary>
        /// permissionGroup分页查询
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("search-result")]
        public async Task<IActionResult> SearchResult(QueryPagePermissionGroupCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 根据permissiongroup-id获取permissionGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _bus.SendAsync(new QueryPermissionGroupCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 根据permissiongroup-name获取permissionGroup
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("name/{name}")]
        public async Task<IActionResult> Name(string name)
        {
            var response = await _bus.SendAsync(new QueryByNamePermissionGroupCommand() { Name = name });
            return Result(response);
        }

        /// <summary>
        /// 新增删除permissiongroup,permission关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id}/permission"), ActionLog("给权限分配权限组")]
        public async Task<IActionResult> AssignPermissionToPermissionGroup(string id, AssignPermissionPermissionGroupCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 根据permissiongroup-id获取permission列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/permission")]
        public async Task<IActionResult> FindPermissionsByPermissionGroupId(string id)
        {
            var response = await _bus.SendAsync(new QueryPermissionByIdPermissionGroupCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 获取所有permissionGroup列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            var response = await _bus.SendAsync(new QueryAllPermissionGroupCommand());
            return Result(response);
        }
    }
}