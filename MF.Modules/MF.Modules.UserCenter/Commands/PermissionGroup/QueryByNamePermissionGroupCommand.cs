using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryByNamePermissionGroupCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }
    }
}