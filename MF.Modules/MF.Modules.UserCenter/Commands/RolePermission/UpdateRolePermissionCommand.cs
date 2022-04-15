using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateRolePermissionCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }

        public UpdateRolePermissionCommand(string Id)
        {
            this.Id = Id;
        }
    }
}