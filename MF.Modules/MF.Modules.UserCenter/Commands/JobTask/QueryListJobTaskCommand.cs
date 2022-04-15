using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands.JobTask
{
    public class QueryListJobTaskCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }
    }
}