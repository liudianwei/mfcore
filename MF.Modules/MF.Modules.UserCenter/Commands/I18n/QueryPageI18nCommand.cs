using MF.FluentValidation;
using MF.MediatR;

namespace MDCenter.Commands.I18n
{
    public class QueryPageI18nCommand : PageCommand<PubResponse>
    {
        public QueryPageI18nCommand()
        {
        }
    }
}