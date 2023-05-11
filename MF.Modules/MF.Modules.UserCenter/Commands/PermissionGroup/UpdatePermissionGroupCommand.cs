using MediatR;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class UpdatePermissionGroupCommand : IRequest<PubResponse>
    {
        public string Id { get; set; }
        public string Name { get;  set; }
        //public string Code { get;  set; }
        public string Remark { get;  set; }
        //public string Updator { get;  set; }

        public UpdatePermissionGroupCommand()
        {
        }
    }
}