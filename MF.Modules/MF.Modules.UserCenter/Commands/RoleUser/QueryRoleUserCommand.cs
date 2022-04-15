using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryRoleUserCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}