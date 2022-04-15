using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateOperationLogCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }

        public UpdateOperationLogCommand(string Id)
        {
            this.Id = Id;
        }
    }
}