using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreatePermissionAndGroupCommand : IRequest<PubResponse>
    {
    }
}