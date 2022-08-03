using System.Collections.Generic;
using System.Threading.Tasks;

using MDCenter.Commands.I18n;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using MF.MediatR;
using MF.NetCoreApp;
using MF.NetCoreApp.Attributes;
using MF.Utils;


namespace MDCenter.Controllers.v1
{
    [Route("rest/usercenter/v{version:apiVersion}/i18n")]
    [ApiVersion("1")]
    public class I18nController : ApiBaseController
    {
        private readonly ILogger<I18nController> _logger;
        private readonly IBus _bus;

        public I18nController(ILogger<I18nController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 创建 多语言配置
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("创建多语言配置")]
        public async Task<IActionResult> Post(CreateI18nCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 根据ID查询 多语言配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _bus.SendAsync(new QueryI18nCommand() { Id = id});
            return Result(response);
        }

        /// <summary>
        /// 根据ID列表查询 多语言配置
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<IActionResult> GetList(List<string> list)
        {
            var response = await _bus.SendAsync(new QueryListI18nCommand() { List = list });
            return Result(response);
        }

        /// <summary>
        /// 查询所有 多语言配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bus.SendAsync(new QueryAllI18nCommand());
            return Result(response);
        }

        /// <summary>
        /// 分页查询 多语言配置
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("search-result")]
        public async Task<IActionResult> Page(QueryPageI18nCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 修改 多语言配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionLog("修改多语言配置")]
        public async Task<IActionResult> Put(string id, UpdateI18nCommand command)
        {
            command.Id = id;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 删除 多语言配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionLog("删除多语言配置")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _bus.SendAsync(new DeleteI18nCommand{ List = new List<string>() { id } });
            return Result(response);
        }

        /// <summary>
        /// 批量删除 多语言配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("batch-delete-request"), ActionLog("批量删除多语言配置")]
        public async Task<IActionResult> Delete(List<string> ids)
        {
            var response = await _bus.SendAsync(new DeleteI18nCommand() { List = ids });
            return Result(response);
        }

        /// <summary>
        /// 修改 多语言配置 状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("state/{id}"), ActionLog("修改多语言配置状态")]
        public async Task<IActionResult> ChangeState(string id)
        {
            var response = await _bus.SendAsync(new SetStateI18nCommand() { Id = id });
            return Result(response);
        }

        /// <summary>
        /// 批量修改 多语言配置 状态
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost("batch-state-request"), ActionLog("批量修改多语言配置状态")]
        public async Task<IActionResult> BatchChangeState(BatchSetStateI18nCommand cmd)
        {
            var response = await _bus.SendAsync(cmd);
            return Result(response);
        }

        /// <summary>
        /// 根据ID查询 多语言配置
        /// </summary>
        /// <param name="netUrl">文件服务器地址</param>
        /// <returns></returns>
        [HttpPost("publish"), ActionLog("发布")]
        public async Task<IActionResult> Publish(string netUrl)
        {
            var response = await _bus.SendAsync(new PublishI18nCommand() { NetUrl= netUrl });
            return Result(response);
        }
    }
}
