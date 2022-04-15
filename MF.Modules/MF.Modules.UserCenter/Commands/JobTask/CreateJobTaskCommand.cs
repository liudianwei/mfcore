using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands.JobTask
{
    public class CreateJobTaskCommand : IRequest<PubResponse>
    {
        public CreateJobTaskCommand()
        {
        }
        
        public string JobType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Target { get; set; }

        public string TargetDetail { get; set; }

        public string Token { get; set; }

        public string Method { get; set; }

        public string JobArgs { get; set; }

        public string CronExpression { get; set; }

        public string CronExpressionDescription { get; set; }

        public int State { get; set; }

    }
}