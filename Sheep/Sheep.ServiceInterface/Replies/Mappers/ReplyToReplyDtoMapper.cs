using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Replies.Entities;

namespace Sheep.ServiceInterface.Replies.Mappers
{
    public static class ReplyToReplyDtoMapper
    {
        public static ReplyDto MapToReplyDto(this Reply reply, IUserAuth user, bool yesVoted, bool noVoted)
        {
            var replyDto = new ReplyDto
                           {
                               Id = reply.Id,
                               ParentType = reply.ParentType,
                               ParentId = reply.ParentId,
                               Content = reply.Content,
                               Status = reply.Status,
                               User = user?.MapToBasicUserDto(),
                               CreatedDate = reply.CreatedDate.ToUnixTime(),
                               ModifiedDate = reply.ModifiedDate.ToUnixTime(),
                               VotesCount = reply.VotesCount,
                               YesVotesCount = reply.YesVotesCount,
                               NoVotesCount = reply.NoVotesCount,
                               YesVoted = yesVoted,
                               NoVoted = noVoted
                           };
            return replyDto;
        }
    }
}