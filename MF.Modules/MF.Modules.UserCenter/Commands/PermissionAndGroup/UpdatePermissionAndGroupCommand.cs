using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdatePermissionAndGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }

        public UpdatePermissionAndGroupCommand(string Id)
        {
            this.Id = Id;
        }
    }
}