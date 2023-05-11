using MediatR;
using System.Collections.Generic;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreateUserCommand : IRequest<PubResponse>
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }
    }
}