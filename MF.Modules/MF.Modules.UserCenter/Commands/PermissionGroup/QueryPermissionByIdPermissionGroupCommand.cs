using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionByIdPermissionGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}