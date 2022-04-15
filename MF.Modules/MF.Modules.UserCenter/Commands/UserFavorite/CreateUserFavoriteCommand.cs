using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreateUserFavoriteCommand : IRequest<PubResponse>
    {
    }
}