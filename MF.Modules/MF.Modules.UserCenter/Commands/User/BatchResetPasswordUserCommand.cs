using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class BatchResetPasswordUserCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }

    }
}