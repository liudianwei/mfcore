using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreateAccessLogCommand : IRequest<PubResponse>
    {
    }
}