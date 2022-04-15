using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class AssignPermissionPermissionGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public List<string> plist { get; set; }
        public string Updator { get; set; }
    }
}