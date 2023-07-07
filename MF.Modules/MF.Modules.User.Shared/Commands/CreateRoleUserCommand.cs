using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreateRoleUserCommand : IRequest<PubResponse>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

    }
}