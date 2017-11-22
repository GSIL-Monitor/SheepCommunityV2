using System.Collections.Generic;
using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Posts.Entities;

namespace Sheep.ServiceInterface.Posts.Mappers
{
    public static class PostToBasicPostDtoMapper
    {
        public static BasicPostDto MapToBasicPostDto(this Post post, IUserAuth author)
        {
            if (post.Meta == null)
            {
                post.Meta = new Dictionary<string, string>();
            }
            var postDto = new BasicPostDto
                          {
                              Id = post.Id,
                              Title = post.Title,
                              Summary = post.Summary,
                              PictureUrl = post.PictureUrl,
                              ContentType = post.ContentType,
                              PublishedDate = post.PublishedDate?.ToUnixTime(),
                              IsFeatured = post.IsFeatured,
                              Author = author.MapToBasicUserDto(),
                              CommentsCount = post.CommentsCount,
                              LikesCount = post.LikesCount,
                              SharesCount = post.SharesCount
                          };
            return postDto;
        }
    }
}