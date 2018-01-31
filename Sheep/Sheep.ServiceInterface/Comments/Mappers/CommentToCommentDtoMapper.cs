using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Comments.Entities;

namespace Sheep.ServiceInterface.Comments.Mappers
{
    public static class CommentToCommentDtoMapper
    {
        public static CommentDto MapToCommentDto(this Comment comment, string title, string pictureUrl, IUserAuth user, bool yesVoted, bool noVoted)
        {
            var commentDto = new CommentDto
                             {
                                 Id = comment.Id,
                                 ParentType = comment.ParentType,
                                 ParentId = comment.ParentId,
                                 ParentTitle = title,
                                 ParentPictureUrl = pictureUrl,
                                 Content = comment.Content,
                                 Status = comment.Status,
                                 User = user?.MapToBasicUserDto(),
                                 CreatedDate = comment.CreatedDate.ToUnixTime(),
                                 ModifiedDate = comment.ModifiedDate.ToUnixTime(),
                                 IsFeatured = comment.IsFeatured,
                                 RepliesCount = comment.RepliesCount,
                                 VotesCount = comment.VotesCount,
                                 YesVotesCount = comment.YesVotesCount,
                                 NoVotesCount = comment.NoVotesCount,
                                 YesVoted = yesVoted,
                                 NoVoted = noVoted
                             };
            return commentDto;
        }
    }
}