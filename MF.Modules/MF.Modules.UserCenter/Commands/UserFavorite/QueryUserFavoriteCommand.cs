using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryUserFavoriteCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
    }
}