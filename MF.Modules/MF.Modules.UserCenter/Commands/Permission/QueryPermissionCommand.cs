using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}