using System;

using MediatR;

using MF.FluentValidation;

namespace MDCenter.Commands.WorkStation
{
    public class QueryUserExistWorkStationCommand : IRequest<PubResponse>
    {
        public QueryUserExistWorkStationCommand()
        {
        }
        public string LineCode { get; set; }
        public string Name { get; set; }

        public string OpName { get; set; }

        public string WorkStationId { get; set; }
    }
}