using MediatR;
using MF.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCenter.Commands
{
    public class IPCCheckBtnUserCommand : IRequest<PubResponse>
    {
        public IPCCheckBtnUserCommand()
        {
        }
        public string Name { get; set; }
        public string Password { get; set; }
        public string BtfunctionCode { get; set; }
        public string OpName { get; set; }
        public string LineCode { get; set; }
    }
}
