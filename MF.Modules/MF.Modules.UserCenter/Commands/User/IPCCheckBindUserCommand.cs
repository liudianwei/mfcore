using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class IPCCheckBindUserCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
