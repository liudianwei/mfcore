using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCenter.Response.User
{
    public class UserLoginResp
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public string LoginType { get; set; }
        public DateTime ExpireTime { get; set; }

        public string UserType { get; set; }
        public string Token { get; set; }
        public bool Enable { get; set; }
        public Additionalinformation AdditionalInformation { get; set; }
    }

    public class Additionalinformation
    {
    }
}