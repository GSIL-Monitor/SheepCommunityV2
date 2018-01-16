using System.Collections.Generic;
using ServiceStack.Auth;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceInterface.Users.Mappers
{
    public static class UserAuthToBasicUserDtoMapper
    {
        public static BasicUserDto MapToBasicUserDto(this IUserAuth userAuth)
        {
            if (userAuth.Meta == null)
            {
                userAuth.Meta = new Dictionary<string, string>();
            }
            var userDto = new BasicUserDto
                          {
                              Id = userAuth.Id,
                              UserName = userAuth.UserName,
                              DisplayName = userAuth.DisplayName,
                              Signature = userAuth.Meta.GetValueOrDefault("Signature"),
                              AvatarUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                              Gender = userAuth.Gender
                          };
            return userDto;
        }
    }
}