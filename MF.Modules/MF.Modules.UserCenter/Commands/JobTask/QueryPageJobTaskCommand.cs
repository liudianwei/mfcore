using MF.FluentValidation;
using MF.MediatR;

namespace UserCenter.Commands.JobTask
{
    public class QueryPageJobTaskCommand : PageCommand<PubResponse>
    {
        public QueryPageJobTaskCommand()
        {
        }
        public string JobType { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
    }
}