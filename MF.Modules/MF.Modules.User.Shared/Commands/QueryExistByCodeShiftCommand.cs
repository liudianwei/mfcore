using MediatR;

using MF.FluentValidation;

namespace MDCenter.Commands.Shift
{
    public class QueryExistByCodeShiftCommand : IRequest<PubResponse>
    {
        public QueryExistByCodeShiftCommand()
        {
        }

        public string Code { get; set; }
    }
}