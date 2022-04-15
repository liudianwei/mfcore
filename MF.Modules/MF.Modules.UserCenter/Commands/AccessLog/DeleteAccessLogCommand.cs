using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeleteAccessLogCommand : IRequest<PubResponse>
    {
        public string Id { get; }

        public DeleteAccessLogCommand(string Id)
        {
            this.Id = Id;
        }
    }
}