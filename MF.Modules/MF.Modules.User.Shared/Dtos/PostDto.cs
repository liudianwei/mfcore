using MF.Orm;

namespace UserCenter.Dtos
{
    public class PostDto : BaseDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int OrderNum { get; set; } = -1;
        public string OrgId { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public string OrgName { get; set; }
        public string Remark { get; set; }
    }
}