using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class AssignUserToRoleRoleCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public List<string> List { get;  set; }
    }
}