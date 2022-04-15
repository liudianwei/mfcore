using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands.JobTask
{
    public class SetStateJobTaskCommand : IRequest<PubResponse>
    {
        public SetStateJobTaskCommand()
        {
        }
        
        public string BackgroundJobId { get; set; }

        public int State { get; set; }

    }
}