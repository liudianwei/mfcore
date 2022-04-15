using System.Collections.Generic;

using MediatR;

using MF.FluentValidation;

namespace UserCenter.Commands.JobTask
{
    public class BatchSetStateJobTaskCommand : IRequest<PubResponse>
    {
        public BatchSetStateJobTaskCommand()
        {
        }
        
        public int State { get; set; }

        public List<string> List { get; set; }
    }
}