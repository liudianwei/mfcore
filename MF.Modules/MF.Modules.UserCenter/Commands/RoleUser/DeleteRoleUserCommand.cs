using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeleteRoleUserCommand : IRequest<PubResponse>
    {
        public string Id { get; }

        public DeleteRoleUserCommand(string Id)
        {
            this.Id = Id;
        }
    }
}