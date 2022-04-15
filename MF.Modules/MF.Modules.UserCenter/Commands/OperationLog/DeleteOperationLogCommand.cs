using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeleteOperationLogCommand : IRequest<PubResponse>
    {
        public string Id { get; }

        public DeleteOperationLogCommand(string Id)
        {
            this.Id = Id;
        }
    }
}