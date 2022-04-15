using MediatR;

using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class LogoutUserCommand : IRequest<PubResponse>
    {
    }
}