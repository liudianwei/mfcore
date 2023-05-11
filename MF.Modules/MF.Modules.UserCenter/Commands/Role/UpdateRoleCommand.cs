using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public string Name { get;  set; }
        public string Remark { get;  set; }

        public UpdateRoleCommand()
        {
        }
    }
}