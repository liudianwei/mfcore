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
    public class PermissionPermissiongroupController : ApiBaseController
    {
        private readonly ILogger<PermissionPermissiongroupController> _logger;
        private readonly IBus _bus;

        public PermissionPermissiongroupController(ILogger<PermissionPermissiongroupController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("关联权限权限组")]
        public async Task<IActionResult> Post(CreatePermissionAndGroupCommand command)
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
            var response = await _bus.SendAsync(new QueryPermissionAndGroupCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionLog("修改权限权限组关联")]
        public async Task<IActionResult> Put(string id, UpdatePermissionAndGroupCommand command)
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
        [HttpDelete("{id}"), ActionLog("删除权限权限组关联")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _bus.SendAsync(new DeletePermissionAndGroupCommand(id));
            return Result(response);
        }
    }
}