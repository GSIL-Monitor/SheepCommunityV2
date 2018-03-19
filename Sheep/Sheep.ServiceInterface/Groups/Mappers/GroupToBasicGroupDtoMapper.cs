using System.Collections.Generic;
using Sheep.Model.Membership.Entities;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceInterface.Groups.Mappers
{
    public static class GroupToBasicGroupDtoMapper
    {
        public static BasicGroupDto MapToBasicGroupDto(this Group group)
        {
            if (group.Meta == null)
            {
                group.Meta = new Dictionary<string, string>();
            }
            var groupDto = new BasicGroupDto
                           {
                               Id = group.Id,
                               DisplayName = group.DisplayName,
                               IconUrl = group.IconUrl
                           };
            return groupDto;
        }
    }
}