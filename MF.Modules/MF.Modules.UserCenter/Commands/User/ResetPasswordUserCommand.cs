using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class ResetPasswordUserCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public string Updator { get; set; }

    }
}