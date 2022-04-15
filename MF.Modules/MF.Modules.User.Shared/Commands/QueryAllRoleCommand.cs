using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryAllRoleCommand : IRequest<PubResponse>
    {
    }
}