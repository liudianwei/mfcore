using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryFavoriteByUserIdUserCommand : IRequest<PubResponse>
    {
        public string Uid { get; set; }

    }
}