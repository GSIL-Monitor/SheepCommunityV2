using System.Collections.Generic;
using ServiceStack.Auth;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Follows.Entities;

namespace Sheep.ServiceInterface.Follows.Mappers
{
    public static class FollowToBasicFollowDtoMapper
    {
        public static BasicFollowOfFollowingUserDto MapToBasicFollowOfFollowingUserDto(this Follow follow, IUserAuth followingUser)
        {
            if (follow.Meta == null)
            {
                follow.Meta = new Dictionary<string, string>();
            }
            var followDto = new BasicFollowOfFollowingUserDto
                            {
                                FollowingUser = followingUser?.MapToBasicUserDto(),
                                IsBidirectional = follow.IsBidirectional
                            };
            return followDto;
        }

        public static BasicFollowOfFollowerDto MapToBasicFollowOfFollowerDto(this Follow follow, IUserAuth follower)
        {
            if (follow.Meta == null)
            {
                follow.Meta = new Dictionary<string, string>();
            }
            var followDto = new BasicFollowOfFollowerDto
                            {
                                Follower = follower?.MapToBasicUserDto(),
                                IsBidirectional = follow.IsBidirectional
                            };
            return followDto;
        }
    }
}