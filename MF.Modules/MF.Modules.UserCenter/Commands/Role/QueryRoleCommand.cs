using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}