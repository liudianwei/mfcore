using System.Collections.Generic;

using MediatR;
using MF.FluentValidation;

namespace MDCenter.Commands.I18n
{
    public class DeleteI18nCommand : IRequest<PubResponse>
    {
        public DeleteI18nCommand()
        {
        }

        public List<string> List { get; set; }
    }
}