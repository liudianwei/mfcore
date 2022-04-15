using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands.JobTask
{
    public class QueryJobTaskCommand : IRequest<PubResponse>
    {
        public QueryJobTaskCommand()
        {
        }
        
        public string BackgroundJobId { get; set; }

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

        public string NextRunTime { get; set; }

        public string LastRunTime { get; set; }

        public int RunCount { get; set; }

        public int State { get; set; }

        public int DisplayOrder { get; set; }

        public string CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public string CreatedDateTime { get; set; }

        public string LastUpdatedByUserId { get; set; }

        public string LastUpdatedByUserName { get; set; }

        public string LastUpdatedDateTime { get; set; }

    }
}