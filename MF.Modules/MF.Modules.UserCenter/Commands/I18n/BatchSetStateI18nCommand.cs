using System.Collections.Generic;

using MediatR;

using MF.FluentValidation;

namespace MDCenter.Commands.I18n
{
    public class BatchSetStateI18nCommand : IRequest<PubResponse>
    {
        public BatchSetStateI18nCommand()
        {
        }
        
        public string State { get; set; }

        public List<string> List { get; set; }
    }
}