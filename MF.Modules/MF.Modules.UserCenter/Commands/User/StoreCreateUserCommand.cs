using MediatR;
using System.Collections.Generic;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class StoreCreateUserCommand : IRequest<PubResponse>
    {
        public UserDto Dto { get;  set; }
    }
}