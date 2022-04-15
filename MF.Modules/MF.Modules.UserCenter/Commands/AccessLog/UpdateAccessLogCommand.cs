using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateAccessLogCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }

        public UpdateAccessLogCommand(string Id)
        {
            this.Id = Id;
        }
    }
}