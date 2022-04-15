using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MF.MediatR;
using UserCenter.Commands;
using MF.NetCoreApp;
using MF.NetCoreApp.Attributes;
using Microsoft.Extensions.Localization;
using MF.Utils;

namespace UserCenter.Controllers.v1
{
    [ApiVersion("1.0")]
    public class RoleUserController : ApiBaseController
    {
        private readonly ILogger<RoleUserController> _logger;
        private readonly IBus _bus;

        public RoleUserController(ILogger<RoleUserController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("创建用户角色关联")]
        public async Task<IActionResult> Post(CreateRoleUserCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _bus.SendAsync(new QueryRoleUserCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionLog("修改用户角色关联")]
        public async Task<IActionResult> Put(string id, UpdateRoleUserCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionLog("删除用户角色关联")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _bus.SendAsync(new DeleteRoleUserCommand(id));
            return Result(response);
        }
    }
}