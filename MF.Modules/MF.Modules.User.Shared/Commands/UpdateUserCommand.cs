using MediatR;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdateUserCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }
    }
}