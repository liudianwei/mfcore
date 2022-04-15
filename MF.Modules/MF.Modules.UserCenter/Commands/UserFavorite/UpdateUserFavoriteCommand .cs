using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateUcUserFavoriteCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }

        public UpdateUcUserFavoriteCommand(string Id)
        {
            this.Id = Id;
        }
    }
}