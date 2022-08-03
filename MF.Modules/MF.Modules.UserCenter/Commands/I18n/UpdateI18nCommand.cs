using MediatR;
using MF.FluentValidation;

namespace MDCenter.Commands.I18n
{
    public class UpdateI18nCommand : IRequest<PubResponse>
    {
        public UpdateI18nCommand()
        {
        }
        
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Context { get; set; }

        public string Language { get; set; }

        public string Path { get; set; }

        public string State { get; set; }

        public string Creator { get; set; }

        public string Updator { get; set; }

        public System.DateTime CreateTime { get; set; }

        public System.DateTime UpdateTime { get; set; }

        public int InnerVersion { get; set; }
        public string Remark { get; set; }

    }
}