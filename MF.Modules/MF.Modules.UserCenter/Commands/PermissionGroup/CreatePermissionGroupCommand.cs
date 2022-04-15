using MediatR;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreatePermissionGroupCommand : IRequest<PubResponse>
    {
        public PermissionGroupDto Dto { get;  set; }
    }
}