using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryRolePermissionCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}