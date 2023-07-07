using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryAccessLogByTimeCommand : IRequest<PubResponse>
    {
        /// <summary>
        /// 请求时间
        /// </summary>
        public System.DateTime? RequestTime { get; set; }
        public string Source { get; set; }
    }
}