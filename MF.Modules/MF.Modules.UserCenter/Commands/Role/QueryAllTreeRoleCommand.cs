using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryAllTreeRoleCommand : IRequest<PubResponse>
    {
    }
}