using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryByUserNameRoleCommand : IRequest<PubResponse>
    {
        public string UserName { get; set; }
    }
    public class QueryByRoleNameCommand : IRequest<PubResponse>
    {
        public string RoleName { get; set; }
    }
}