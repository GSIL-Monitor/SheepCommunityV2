using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Likes.Entities;

namespace Sheep.ServiceInterface.Likes.Mappers
{
    public static class LikeToLikeDtoMapper
    {
        public static LikeDto MapToLikeDto(this Like like, string title, string pictureUrl, string contentType, IUserAuth user)
        {
            var likeDto = new LikeDto
                          {
                              ParentType = like.ParentType,
                              ParentId = like.ParentId,
                              ParentTitle = title,
                              ParentPictureUrl = pictureUrl,
                              ParentContentType = contentType,
                              User = user?.MapToBasicUserDto(),
                              CreatedDate = like.CreatedDate.ToUnixTime()
                          };
            return likeDto;
        }
    }
}