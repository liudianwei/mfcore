using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryUserListByNameCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }
    }
}