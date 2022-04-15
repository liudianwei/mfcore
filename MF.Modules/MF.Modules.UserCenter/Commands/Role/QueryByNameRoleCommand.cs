using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryByNameRoleCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }
    }
}