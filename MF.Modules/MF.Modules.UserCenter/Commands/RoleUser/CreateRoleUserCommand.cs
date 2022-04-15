using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreateRoleUserCommand : IRequest<PubResponse>
    {
    }
}