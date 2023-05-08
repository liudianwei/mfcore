using System.Collections.Generic;
using System.Threading.Tasks;

using UserCenter.Commands.JobTask;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using MF.MediatR;
using MF.NetCoreApp;
using MF.NetCoreApp.Attributes;
using MF.Utils;
using System.IO;
using System;
using MF.FluentValidation;

namespace UserCenter.Controllers.v1
{
    [Route("rest/usercenter/v{version:apiVersion}/jobtask")]
    [ApiVersion("1")]
    public class JobTaskController : ApiBaseController
    {
        private readonly ILogger<JobTaskController> _logger;
        private readonly IBus _bus;

        public JobTaskController(ILogger<JobTaskController> logger, IBus bus, IStringLocalizer<Msg> localizer) : base(localizer)
        {
            _logger = logger;
            _bus = bus;
        }

        /// <summary>
        /// 创建 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost, ActionLog("【任务调度】创建")]
        public async Task<IActionResult> Post(CreateJobTaskCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 根据ID查询 
        /// </summary>
        /// <param name="backgroundJobId"></param>
        /// <returns></returns>
        [HttpGet("{backgroundJobId}")]
        public async Task<IActionResult> Get(string backgroundJobId)
        {
            var response = await _bus.SendAsync(new QueryJobTaskCommand() { BackgroundJobId = backgroundJobId });
            return Result(response);
        }

        /// <summary>
        /// 根据ID列表查询 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<IActionResult> GetList(List<string> list)
        {
            var response = await _bus.SendAsync(new QueryListJobTaskCommand() { List = list });
            return Result(response);
        }

        /// <summary>
        /// 查询所有 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bus.SendAsync(new QueryAllJobTaskCommand());
            return Result(response);
        }

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("search-result")]
        public async Task<IActionResult> Page(QueryPageJobTaskCommand command)
        {
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 修改 
        /// </summary>
        /// <param name="backgroundJobId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{backgroundJobId}"), ActionLog("【任务调度】修改")]
        public async Task<IActionResult> Put(string backgroundJobId, UpdateJobTaskCommand command)
        {
            command.BackgroundJobId = backgroundJobId;
            var response = await _bus.SendAsync(command);
            return Result(response);
        }

        /// <summary>
        /// 删除 
        /// </summary>
        /// <param name="backgroundJobId"></param>
        /// <returns></returns>
        [HttpDelete("{backgroundJobId}"), ActionLog("【任务调度】删除")]
        public async Task<IActionResult> Delete(string backgroundJobId)
        {
            var response = await _bus.SendAsync(new DeleteJobTaskCommand { List = new List<string>() { backgroundJobId } });
            return Result(response);
        }

        /// <summary>
        /// 批量删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("batch-delete-request"), ActionLog("【任务调度】批量删除")]
        public async Task<IActionResult> Delete(List<string> ids)
        {
            var response = await _bus.SendAsync(new DeleteJobTaskCommand() { List = ids });
            return Result(response);
        }

        /// <summary>
        /// 修改 job运行状态  3-请求启动  5-请求停止
        /// </summary>
        /// <param name="backgroundJobId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost("state/{backgroundJobId}/{state}"), ActionLog("【任务调度】修改状态")]
        public async Task<IActionResult> ChangeState(string backgroundJobId, int state)
        {
            var response = await _bus.SendAsync(new SetStateJobTaskCommand() { BackgroundJobId = backgroundJobId, State = state });
            return Result(response);
        }

        /// <summary>
        /// 批量修改 job运行状态
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost("batch-state-request"), ActionLog("【任务调度】批量修改状态")]
        public async Task<IActionResult> BatchChangeState(BatchSetStateJobTaskCommand cmd)
        {
            var response = await _bus.SendAsync(cmd);
            return Result(response);
        }

        /// <summary>
        /// 上传job的dll，只有类型为内部或外部dll的才要上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload-job-file"), ActionLog("【任务调度】上传job的dll")]
        public async Task<IActionResult> UploadJobDll()
        {
            var response = PubResponse.Succeed();
            var file = Request.Form.Files[0];
            await Task.Delay(10);
            var stream = file.OpenReadStream();
            if (!file.FileName.ToLower().EndsWith(".dll"))
            {
                response = PubResponse.Failed("请上传dll文件");
                return Result(response);
            }
            if(System.IO.File.Exists(Path.Combine(Environment.CurrentDirectory, file.FileName))){
                response = PubResponse.Failed("文件已经存在，请换一个dll名称");
                return Result(response);
            }

            byte[] buffer = new byte[file.Length];
            MemoryStream stmMemory = new MemoryStream();
            int length;
            while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                stmMemory.Write(buffer, 0, length);
            }

            var templicense = Environment.CurrentDirectory + $"\\{file.FileName}";

            FileStream fs = new FileStream(templicense, FileMode.OpenOrCreate);
            stmMemory.WriteTo(fs);
            stmMemory.Close();
            fs.Close();

            return Result(response);
        }
    }
}
