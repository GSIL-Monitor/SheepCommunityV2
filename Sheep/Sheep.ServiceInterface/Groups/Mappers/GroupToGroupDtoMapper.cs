using System.Collections.Generic;
using ServiceStack.Text;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceModel.Groups.Entities;

namespace Sheep.ServiceInterface.Groups.Mappers
{
    public static class GroupToGroupDtoMapper
    {
        public static GroupDto MapToGroupDto(this Group group)
        {
            if (group.Meta == null)
            {
                group.Meta = new Dictionary<string, string>();
            }
            var groupDto = new GroupDto
                           {
                               Id = group.Id,
                               DisplayName = group.DisplayName,
                               FullName = group.FullName,
                               FullNameVerified = group.FullNameVerified,
                               Description = group.Description,
                               IconUrl = group.IconUrl,
                               CoverPhotoUrl = group.CoverPhotoUrl,
                               CreatedDate = group.CreatedDate.ToUnixTime(),
                               ModifiedDate = group.ModifiedDate.ToUnixTime()
                           };
            return groupDto;
        }
    }
}