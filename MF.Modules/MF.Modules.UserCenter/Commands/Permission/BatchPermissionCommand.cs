using MediatR;
using System.Collections.Generic;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class BatchPermissionCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public string ParentName { get; set; }
        public List<PermissionBtnDto> List { get; set; }
    }
}