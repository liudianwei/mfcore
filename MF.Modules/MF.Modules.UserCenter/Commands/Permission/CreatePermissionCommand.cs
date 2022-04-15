using MediatR;
using System.Collections.Generic;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class CreatePermissionCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public PermissionDto Dto { get; set; }
    }
}