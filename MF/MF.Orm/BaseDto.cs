using System;

namespace MF.Orm
{
    public class BaseDto
    {
        public virtual string Id { get; set; }
        public virtual int InnerVersion { get; set; }
        public virtual string State { get; set; }
        //public virtual string TenantCode { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime? CreateTime { get; set; }

        public virtual string Updator { get; set; }

        public virtual DateTime? UpdateTime { get; set; }
    }
}