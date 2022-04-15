using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeletePermissionCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }

        public DeletePermissionCommand()
        {
        }
    }
}