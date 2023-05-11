using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreateRoleCommand : IRequest<PubResponse>
    {
        public string Name { get;  set; }
        public string Remark { get;  set; }
    }
}