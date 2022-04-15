using MediatR;
using System.Data;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class ImportUserCommand : IRequest<PubResponse>
    {
        public DataTable dataTable { get; set; }
    }
}