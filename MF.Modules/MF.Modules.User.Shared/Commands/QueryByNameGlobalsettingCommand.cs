using MediatR;

using MF.FluentValidation;
using System.Collections.Generic;

namespace MDCenter.Commands.Globalsetting
{
    public class QueryByNameGlobalsettingCommand : IRequest<PubResponse>
    {
        public QueryByNameGlobalsettingCommand()
        {
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Remark { get; set; }
        public List<QueryByNameGlobalsettingCommand> SysModelItemDtoList { get; set; }
    }
}