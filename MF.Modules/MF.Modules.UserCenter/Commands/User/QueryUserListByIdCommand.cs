using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryUserListByIdCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}