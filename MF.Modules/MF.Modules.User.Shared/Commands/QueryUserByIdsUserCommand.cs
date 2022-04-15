using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class QueryUserByIdsUserCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }
    }
}