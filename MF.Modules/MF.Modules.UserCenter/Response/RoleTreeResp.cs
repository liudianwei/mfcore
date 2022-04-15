using System.Collections.Generic;

namespace UserCenter.Response
{
    public class RoleTreeResp
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<DAL.UserCenter.Entities.User> children { set; get; } = new List<DAL.UserCenter.Entities.User>();
    }
}