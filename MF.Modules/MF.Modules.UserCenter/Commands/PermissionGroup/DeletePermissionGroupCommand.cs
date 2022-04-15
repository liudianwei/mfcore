using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class DeletePermissionGroupCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }

        public DeletePermissionGroupCommand(List<string> Ids)
        {
            this.List = Ids;
        }
    }
}