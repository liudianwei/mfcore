using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryUserCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}