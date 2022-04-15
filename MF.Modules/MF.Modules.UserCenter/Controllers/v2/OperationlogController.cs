using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MF.MediatR;
using UserCenter.Commands;
using MF.NetCoreApp;
using Common.Commands;
using Microsoft.Extensions.Localization;
using MF.Utils;

namespace UserCenter.Controllers.v2
{
    [Route("/rest/usercenter/v{version:apiVersion}/operationlog")]
    [ApiVersion("2")]
    public class OperationlogController : ApiBaseController
    {
        private readonly ILogger<OperationlogController> _logger;
        private readonly IBus _bus;

        public OperationlogController(ILogger<OperationlogController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// operationlog创建
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateOperationLogCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// operationlog分页查询
        /// </summary>
        /// /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("search-result")]
        public async Task<IActionResult> SearchResult(QueryPageOperationLogCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }
    }
}