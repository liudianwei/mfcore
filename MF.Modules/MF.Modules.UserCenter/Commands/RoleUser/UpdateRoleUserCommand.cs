using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateRoleUserCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }

        public UpdateRoleUserCommand(string Id)
        {
            this.Id = Id;
        }
    }
}