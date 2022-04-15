using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionsByRoleIdRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}