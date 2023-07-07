using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionsByRoleIdGroupRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public string PermissionGroupName { get; set; }
    }
}
