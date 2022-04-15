using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class StoreBatchResetPasswordUserCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }

    }
}