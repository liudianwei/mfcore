using MediatR;
using System.Collections.Generic;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class AssignFavoriteUserCommand : IRequest<PubResponse>
    {
        public List<UserFavoriteDto> List { get; set; }

    }
}