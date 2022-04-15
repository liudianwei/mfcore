using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryUsersByRoleIdRoleRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}