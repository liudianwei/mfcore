using System.Collections.Generic;

namespace UserCenter.Dtos
{
    public class PostTreeResp : PostDto
    {
        public int Level { get; set; }

        public List<PostTreeResp> Children { set; get; } = new List<PostTreeResp>();
    }

}