using DAL.UserCenter.Entities;
using MF.Orm;
using System.Collections.Generic;

namespace UserCenter.Dtos
{
    public class RoleDto
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class UserRDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public List<Role> RoleList { get; set; }
    }
    public class RoleUDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public List<User> UserList { get; set; }
    }
    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public object RoleList { get; set; }
    }
    public class RoleInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}