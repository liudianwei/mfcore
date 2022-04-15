using MediatR;

using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionTreeByRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}