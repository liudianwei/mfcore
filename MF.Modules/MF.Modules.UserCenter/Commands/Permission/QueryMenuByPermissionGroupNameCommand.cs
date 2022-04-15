using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryMenuByPermissionGroupNameCommand : IRequest<PubResponse>
    {
        public string UserName { get; set; }
        public string PermissionGroupName { get; set; }
    }
}