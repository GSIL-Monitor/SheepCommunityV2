using System.Collections.Generic;
using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Membership.Entities;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceInterface.Users.Mappers
{
    public static class UserRankToUserRankDtoMapper
    {
        public static UserRankDto MapToUserRankDto(this UserRank userRank, IUserAuth user)
        {
            if (userRank.Meta == null)
            {
                userRank.Meta = new Dictionary<string, string>();
            }
            var userRankDto = new UserRankDto
                              {
                                  User = user?.MapToBasicUserDto(),
                                  LastPostViewsCount = userRank.LastPostViewsCount,
                                  LastPostViewsRank = userRank.LastPostViewsRank,
                                  PostViewsCount = userRank.PostViewsCount,
                                  PostViewsRank = userRank.PostViewsRank,
                                  LastParagraphViewsCount = userRank.LastParagraphViewsCount,
                                  LastParagraphViewsRank = userRank.LastParagraphViewsRank,
                                  ParagraphViewsCount = userRank.ParagraphViewsCount,
                                  ParagraphViewsRank = userRank.ParagraphViewsRank,
                                  CreatedDate = userRank.CreatedDate.ToUnixTime(),
                                  ModifiedDate = userRank.ModifiedDate.ToUnixTime()
                              };
            return userRankDto;
        }
    }
}