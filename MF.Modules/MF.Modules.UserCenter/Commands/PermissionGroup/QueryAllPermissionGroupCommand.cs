using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryAllPermissionGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}