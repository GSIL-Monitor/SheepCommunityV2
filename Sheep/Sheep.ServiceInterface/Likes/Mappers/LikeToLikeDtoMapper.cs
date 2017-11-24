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
                              ContentType = like.ContentType,
                              ContentId = like.ContentId,
                              User = user?.MapToBasicUserDto(),
                              CreatedDate = like.CreatedDate.ToUnixTime()
                          };
            return likeDto;
        }
    }
}