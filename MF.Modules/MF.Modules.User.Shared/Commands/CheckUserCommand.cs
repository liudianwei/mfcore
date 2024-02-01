using MediatR;
using MF.FluentValidation;

namespace MF.Modules.UserCenter.Commands.User
{
    public class CheckUserCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string CheckFrom { get; set; }
    }
}
