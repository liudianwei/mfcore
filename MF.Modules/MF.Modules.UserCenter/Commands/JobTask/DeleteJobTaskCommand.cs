using System.Collections.Generic;

using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands.JobTask
{
    public class DeleteJobTaskCommand : IRequest<PubResponse>
    {
        public DeleteJobTaskCommand()
        {
        }

        public List<string> List { get; set; }
    }
}