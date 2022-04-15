using MediatR;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateUserCommand : IRequest<PubResponse>
    {
        public UserDto Dto { get;  set; }
    }
}