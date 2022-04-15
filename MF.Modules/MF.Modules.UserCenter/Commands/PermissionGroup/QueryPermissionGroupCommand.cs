using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}