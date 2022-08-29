using MediatR;
using MF.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDCenter.Commands.WorkstaionBtfunction
{
    public class CheckWorkstaionBtfunctionCommand : IRequest<PubResponse>
    {
        public CheckWorkstaionBtfunctionCommand() {
        }
        public string BtfunctionCode { get; set; }
        public string OpName { get; set; }
        public string LineCode { get; set; }
    }
    public class VerifyCommand : IRequest<PubResponse>
    {
        public VerifyCommand()
        {
        }
        public string LineCode { get; set; }
        public string OpName { get; set; }
    }
}
