using System.Collections.Generic;

using MediatR;

using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryListPermissionCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }
    }
}