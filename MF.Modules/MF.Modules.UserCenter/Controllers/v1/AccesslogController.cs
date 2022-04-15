using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MF.MediatR;
using UserCenter.Commands;
using MF.NetCoreApp;
using Microsoft.Extensions.Localization;
using MF.Utils;

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
    }
}