using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryByNameUserCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }
    }
}