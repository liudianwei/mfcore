using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UserCenter.Dtos
{
    public class PermissionTreeResp : PermissionDto
    {
        public string ParentName { get; set; }
        public int Level { get; set; }

        public List<PermissionTreeResp> Children { set; get; } = new List<PermissionTreeResp>();

        [JsonIgnore]
        public Dictionary<string, PermissionTreeResp> Temp = new Dictionary<string, PermissionTreeResp>();

    }
}