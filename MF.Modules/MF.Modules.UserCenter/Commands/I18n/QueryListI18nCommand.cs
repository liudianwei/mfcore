using MediatR;
using System.Collections.Generic;
using MF.FluentValidation;

namespace MDCenter.Commands.I18n
{
    public class QueryListI18nCommand : IRequest<PubResponse>
    {
        public List<string> List { get; set; }
    }
}