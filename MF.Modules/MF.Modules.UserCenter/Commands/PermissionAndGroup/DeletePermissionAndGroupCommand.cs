using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeletePermissionAndGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; }

        public DeletePermissionAndGroupCommand(string Id)
        {
            this.Id = Id;
        }
    }
}