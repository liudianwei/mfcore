using System.Collections.Generic;

using MF.Orm;

namespace UserCenter.Dtos
{
    public class PermissionDto : BaseDto
    {
        public string ParentId { get; set; }
        public string Name { get;  set; }
        public string PermissionGroupId { get;  set; }
        public string PermissionGroupName { get;  set; }
        public List<string> PermissionGroupIdList { get;  set; }
        public string Code { get;  set; }
        public string Url { get;  set; }
        public int? OrderNum { get;  set; }
        public string Perms { get;  set; }
        public string Type { get;  set; }
        public string Icon { get;  set; }
        public string ApiUrl { get;  set; }
        public string Remark { get; set; }
    }
    //数据去重
    public class PermissionDtoComparer<T> : IEqualityComparer<T> where T : BaseDto, new()
    {
        public bool Equals(T x, T y)
        {
            if (x == null)
                return y == null;
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            if (obj == null)
                return 0;
            return obj.Id.GetHashCode();
        }
    }


}