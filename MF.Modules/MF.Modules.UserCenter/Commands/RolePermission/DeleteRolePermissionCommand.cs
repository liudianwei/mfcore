using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeleteRolePermissionCommand : IRequest<PubResponse>
    {
        public string Id { get; }

        public DeleteRolePermissionCommand(string Id)
        {
            this.Id = Id;
        }
    }
}