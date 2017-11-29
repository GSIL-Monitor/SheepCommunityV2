using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Votes.Entities;

namespace Sheep.ServiceInterface.Votes.Mappers
{
    public static class VoteToVoteDtoMapper
    {
        public static VoteDto MapToVoteDto(this Vote vote, IUserAuth user)
        {
            var voteDto = new VoteDto
                          {
                              ParentType = vote.ParentType,
                              ParentId = vote.ParentId,
                              Value = vote.Value,
                              User = user?.MapToBasicUserDto(),
                              CreatedDate = vote.CreatedDate.ToUnixTime(),
                              ModifiedDate = vote.ModifiedDate.ToUnixTime()
                          };
            return voteDto;
        }
    }
}