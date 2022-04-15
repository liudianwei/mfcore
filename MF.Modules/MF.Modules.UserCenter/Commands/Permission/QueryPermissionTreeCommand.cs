using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionTreeCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}