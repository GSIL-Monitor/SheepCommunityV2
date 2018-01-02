using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceInterface.Users.Mappers
{
    public static class UserAuthToUserDtoMapper
    {
        public static UserDto MapToUserDto(this IUserAuth userAuth)
        {
            if (userAuth.Meta == null)
            {
                userAuth.Meta = new Dictionary<string, string>();
            }
            var userDto = new UserDto
                          {
                              Id = userAuth.Id,
                              UserName = userAuth.UserName,
                              Email = userAuth.Email,
                              DisplayName = userAuth.DisplayName,
                              FullName = userAuth.FullName,
                              FullNameVerified = userAuth.Meta.GetValueOrDefault("FullNameVerified").To(false),
                              Signature = userAuth.Meta.GetValueOrDefault("Signature"),
                              Description = userAuth.Meta.GetValueOrDefault("Description"),
                              AvatarUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                              CoverPhotoUrl = userAuth.Meta.GetValueOrDefault("CoverPhotoUrl"),
                              BirthDate = userAuth.BirthDate?.ToString("yyyy-MM-dd"),
                              Gender = userAuth.Gender,
                              Country = userAuth.Country,
                              State = userAuth.State,
                              City = userAuth.City,
                              Guild = userAuth.Meta.GetValueOrDefault("Guild"),
                              Status = userAuth.Meta.GetValueOrDefault("Status"),
                              CreatedDate = userAuth.CreatedDate.ToUnixTime(),
                              ModifiedDate = userAuth.ModifiedDate.ToUnixTime(),
                              LockedDate = userAuth.LockedDate?.ToUnixTime(),
                              Points = 0
                          };
            return userDto;
        }
    }
}