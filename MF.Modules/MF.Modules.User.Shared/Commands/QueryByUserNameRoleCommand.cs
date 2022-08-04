using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryByUserNameRoleCommand : IRequest<PubResponse>
    {
        public string UserName { get; set; }
    }
}