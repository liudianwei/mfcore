using MF.Orm;

namespace UserCenter.Dtos
{
    public class PermissionGroupDto : BaseDto
    {
        public string Name { get;  set; }
        public string Code { get;  set; }
        public string Remark { get; set; }
    }
}