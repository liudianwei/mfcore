using MediatR;
using MF.FluentValidation;
using System.Collections.Generic;

namespace UserCenter.Commands
{
    public class UpdateStatusUserCommand : IRequest<PubResponse>
    {
        //public string Id { get; set; }
        //public string TransitionName { get;  set; }
        public UpdateStatusUserCommand()
        {
        }

        public List<string> List { get; set; }
        public string State { get; set; }
    }
}