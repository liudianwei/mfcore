using MediatR;

using System;

using MF.FluentValidation;

namespace Common.Commands
{
    public class CreateOperationLogCommand : IRequest<PubResponse>
    {
        public string BusinessName { get; set; }

        public int Result { get; set; }

        public string Header { get; set; }

        public string Params { get; set; }

        public string Response { get; set; }

        public string Url { get; set; }

        public string Remark { get; set; }

        public string Ip { get; set; }

        public string IpRegion { get; set; }
    }
}