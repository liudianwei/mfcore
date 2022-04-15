using MediatR;

using System.Collections.Generic;

using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class StoreDeleteUserCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }
    }
}