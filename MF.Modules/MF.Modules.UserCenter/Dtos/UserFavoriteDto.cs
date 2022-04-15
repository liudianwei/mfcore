using MF.Orm;

namespace UserCenter.Dtos
{
    public class UserFavoriteDto : BaseDto
    {
        public string Icon { get; set; }

        public string Color { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int DisplayIndex { get; set; }

        public string OwnerId { get; set; }
    }
}