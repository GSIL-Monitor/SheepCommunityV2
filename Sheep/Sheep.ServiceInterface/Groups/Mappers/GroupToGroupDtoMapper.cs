using System.Collections.Generic;
using ServiceStack.Text;
using Sheep.Model.Corp.Entities;
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
                               Type = group.Meta.GetValueOrDefault("Type"),
                               DisplayName = group.DisplayName,
                               FullName = group.FullName,
                               FullNameVerified = group.FullNameVerified,
                               Description = group.Description,
                               IconUrl = group.IconUrl,
                               CoverPhotoUrl = group.CoverPhotoUrl,
                               RefId = group.RefId,
                               Country = group.Country,
                               State = group.State,
                               City = group.City,
                               JoinMode = group.JoinMode,
                               IsPublic = group.IsPublic,
                               EnableMessages = group.EnableMessages,
                               Status = group.Status,
                               BanReason = group.BanReason,
                               BannedUntilDate = group.BannedUntilDate?.ToUnixTime(),
                               CreatedDate = group.CreatedDate.ToUnixTime(),
                               ModifiedDate = group.ModifiedDate.ToUnixTime(),
                               TotalMembers = 0
                           };
            return groupDto;
        }
    }
}