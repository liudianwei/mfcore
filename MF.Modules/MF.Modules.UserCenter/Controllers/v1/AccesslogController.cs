using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MF.MediatR;
using UserCenter.Commands;
using MF.NetCoreApp;
using Microsoft.Extensions.Localization;
using MF.Utils;
using Microsoft.AspNetCore.Authorization;

namespace UserCenter.Controllers.v1
{
    [Route("/rest/usercenter/v{version:apiVersion}/accesslog")]
    [ApiVersion("1")]
    public class AccesslogController : ApiBaseController
    {
        private readonly ILogger<AccesslogController> _logger;
        private readonly IBus _bus;

        public AccesslogController(ILogger<AccesslogController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 创建accesslog
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateAccessLogCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// accesslog分页查询
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("search-result")]
        public async Task<IActionResult> SearchResult(QueryPageAccessLogCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// accesslog根据时间段返回时间(获取员工上下岗位时间)
        /// </summary>
        /// <param name="requestTime">RequestTime时间为空的话 默认返回3天内数据</param>
        /// <param name="source">返回什么来源的数据(IPC/WEB) 参数为空返回所有类型数据</param>
        /// <returns></returns>
        [HttpGet("search-result-reqtime")]
        //[AllowAnonymous]
        public async Task<IActionResult> SearchResultByTime(System.DateTime requestTime,string source)
        {
            var response = await _bus.SendAsync(new QueryAccessLogByTimeCommand() { RequestTime = requestTime, Source = source });
            return Result(response);
        }
    }
}