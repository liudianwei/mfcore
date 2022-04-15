using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class AssignPermissionToRoleRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public List<string> List { get;  set; }
        public string Updator { get;  set; }
    }
}