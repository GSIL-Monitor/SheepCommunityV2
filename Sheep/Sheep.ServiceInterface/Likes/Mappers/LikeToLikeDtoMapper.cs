using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Likes.Entities;

namespace Sheep.ServiceInterface.Likes.Mappers
{
    public static class LikeToLikeDtoMapper
    {
        public static LikeDto MapToLikeDto(this Like like, IUserAuth user)
        {
            var likeDto = new LikeDto
                          {
                              ParentType = like.ParentType,
                              ParentId = like.ParentId,
                              User = user?.MapToBasicUserDto(),
                              CreatedDate = like.CreatedDate.ToUnixTime()
                          };
            return likeDto;
        }
    }
}