using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class ExportUserCommand : IRequest<PubResponse>
    {
        public string Order { get; set; }
        public string Condition { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }
}