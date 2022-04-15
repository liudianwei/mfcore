using System;

using MediatR;

using MF.FluentValidation;

namespace MDCenter.Commands.WorkStation
{
    public class QueryExistByNameWorkStationCommand : IRequest<PubResponse>
    {
        public QueryExistByNameWorkStationCommand()
        {
        }

        public string Name { get; set; }
        public string LineCode { get; set; }
    }
}