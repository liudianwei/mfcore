using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryAllUserCommand : IRequest<PubResponse>
    {
        public List<string> Ids { get; set; }
    }
}