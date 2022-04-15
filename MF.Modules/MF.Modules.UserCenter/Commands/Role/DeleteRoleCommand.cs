using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeleteRoleCommand : IRequest<PubResponse>
    {
        public List<string> List { get; }
        public string Id { get; }

        public DeleteRoleCommand(List<string> ids)
        {
            this.List = ids;
        }
    }
}