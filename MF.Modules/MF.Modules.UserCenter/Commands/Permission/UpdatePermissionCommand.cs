using MediatR;
using System.Collections.Generic;
using UserCenter.Dtos;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdatePermissionCommand : IRequest<PubResponse>
    {
        public string Id { get;  set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string PermissionGroupId { get; set; }
        public string PermissionGroupName { get; set; }
        public List<string> PermissionGroupIdList { get; set; }
        //public string Code { get; set; }
        public string Url { get; set; }
        public int? OrderNum { get; set; }
        //public string Perms { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        //public string ApiUrl { get; set; }
        public string IframeUrl { get; set; }
        //public string Remark { get; set; }

        public UpdatePermissionCommand()
        {
        }

    }
}