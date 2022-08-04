using MediatR;
using MF.FluentValidation;

namespace MDCenter.Commands.I18n
{
    public class QueryI18nCommand : IRequest<PubResponse>
    {
        public QueryI18nCommand()
        {
        }
        
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Context { get; set; }

        public string Language { get; set; }

        public string Path { get; set; }
        public string Remark { get; set; }

    }
}