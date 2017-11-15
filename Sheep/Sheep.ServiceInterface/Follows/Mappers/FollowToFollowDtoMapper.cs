using System.Collections.Generic;
using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Follows.Entities;

namespace Sheep.ServiceInterface.Follows.Mappers
{
    public static class FollowToFollowDtoMapper
    {
        public static FollowDto MapToFollowDto(this Follow follow, IUserAuth followingUser, IUserAuth follower)
        {
            if (follow.Meta == null)
            {
                follow.Meta = new Dictionary<string, string>();
            }
            var followDto = new FollowDto
                            {
                                FollowingUser = followingUser?.MapToBasicUserDto(),
                                Follower = follower?.MapToBasicUserDto(),
                                IsBidirectional = follow.IsBidirectional,
                                CreatedDate = follow.CreatedDate.ToUnixTime(),
                                ModifiedDate = follow.ModifiedDate.ToUnixTime()
                            };
            return followDto;
        }
    }
}