using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryButtonByPermissionGroupNameCommand : IRequest<PubResponse>
    {
        public string PermissionGroupName { get; set; }
    }
}