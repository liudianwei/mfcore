using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreateRolePermissionCommand : IRequest<PubResponse>
    {
    }
}