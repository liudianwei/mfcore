using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryPermissionAndGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}