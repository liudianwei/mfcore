using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeleteUcUserFavoriteCommand : IRequest<PubResponse>
    {
        public string Id { get; }

        public DeleteUcUserFavoriteCommand(string Id)
        {
            this.Id = Id;
        }
    }
}