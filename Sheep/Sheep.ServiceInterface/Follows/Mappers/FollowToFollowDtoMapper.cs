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
        public static FollowDto MapToFollowDto(this Follow follow, IUserAuth owner, IUserAuth follower)
        {
            if (follow.Meta == null)
            {
                follow.Meta = new Dictionary<string, string>();
            }
            var followDto = new FollowDto
                            {
                                Owner = owner?.MapToUserDto(),
                                Follower = follower?.MapToUserDto(),
                                IsBidirectional = follow.IsBidirectional,
                                CreatedDate = follow.CreatedDate.ToUnixTime(),
                                ModifiedDate = follow.ModifiedDate.ToUnixTime()
                            };
            return followDto;
        }

        public static FollowOfOwnerDto MapToFollowOfOwnerDto(this Follow follow, IUserAuth owner)
        {
            if (follow.Meta == null)
            {
                follow.Meta = new Dictionary<string, string>();
            }
            var followDto = new FollowOfOwnerDto
                            {
                                Owner = owner?.MapToUserDto(),
                                IsBidirectional = follow.IsBidirectional,
                                CreatedDate = follow.CreatedDate.ToUnixTime(),
                                ModifiedDate = follow.ModifiedDate.ToUnixTime()
                            };
            return followDto;
        }

        public static FollowOfFollowerDto MapToFollowOfFollowerDto(this Follow follow, IUserAuth follower)
        {
            if (follow.Meta == null)
            {
                follow.Meta = new Dictionary<string, string>();
            }
            var followDto = new FollowOfFollowerDto
                            {
                                Follower = follower?.MapToUserDto(),
                                IsBidirectional = follow.IsBidirectional,
                                CreatedDate = follow.CreatedDate.ToUnixTime(),
                                ModifiedDate = follow.ModifiedDate.ToUnixTime()
                            };
            return followDto;
        }
    }
}