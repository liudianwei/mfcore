using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class LoginUserCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string LoginType { get; set; }
        public bool LoginFalse { get; set; } = false;
    }
}