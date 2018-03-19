using System.Collections.Generic;
using ServiceStack.Text;
using Sheep.Model.Membership.Entities;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceInterface.Groups.Mappers
{
    public static class GroupRankToGroupRankDtoMapper
    {
        public static GroupRankDto MapToGroupRankDto(this GroupRank groupRank, Group group)
        {
            if (groupRank.Meta == null)
            {
                groupRank.Meta = new Dictionary<string, string>();
            }
            var groupRankDto = new GroupRankDto
                               {
                                   Group = group?.MapToBasicGroupDto(),
                                   LastPostViewsCount = groupRank.LastPostViewsCount,
                                   LastPostViewsRank = groupRank.LastPostViewsRank,
                                   PostViewsCount = groupRank.PostViewsCount,
                                   PostViewsRank = groupRank.PostViewsRank,
                                   LastParagraphViewsCount = groupRank.LastParagraphViewsCount,
                                   LastParagraphViewsRank = groupRank.LastParagraphViewsRank,
                                   ParagraphViewsCount = groupRank.ParagraphViewsCount,
                                   ParagraphViewsRank = groupRank.ParagraphViewsRank,
                                   CreatedDate = groupRank.CreatedDate.ToUnixTime(),
                                   ModifiedDate = groupRank.ModifiedDate.ToUnixTime()
                               };
            return groupRankDto;
        }
    }
}