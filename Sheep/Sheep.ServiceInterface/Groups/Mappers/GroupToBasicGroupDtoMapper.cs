using System.Collections.Generic;
using Sheep.Model.Corp.Entities;
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
                               IconUrl = group.IconUrl,
                               RefId = group.RefId,
                               JoinMode = group.JoinMode
                           };
            return groupDto;
        }
    }
}