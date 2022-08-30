
using MF.Orm;

namespace UserCenter.Dtos
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }

        //public string Title { get; set; }

        public string Theme { get; set; }

        //public string IsPwdExpire { get; set; }

        public string RoleName { get; set; }

        public string RoleId { get; set; }

        //public string OldPassword { get; set; }

        //public string NewPassword { get; set; }

        //public string UserType { get; set; }
    }
}