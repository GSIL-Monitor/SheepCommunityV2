using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Posts.Mappers;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.PostBlocks.Entities;

namespace Sheep.ServiceInterface.PostBlocks.Mappers
{
    public static class PostBlockToPostBlockDtoMapper
    {
        public static PostBlockDto MapToPostBlockDto(this PostBlock postBlock, Post post, IUserAuth postAuthor, IUserAuth blocker)
        {
            var postBlockDto = new PostBlockDto
                               {
                                   Post = post.MapToBasicPostDto(postAuthor),
                                   Blocker = blocker?.MapToBasicUserDto(),
                                   Reason = postBlock.Reason,
                                   CreatedDate = postBlock.CreatedDate.ToUnixTime(),
                                   ModifiedDate = postBlock.ModifiedDate.ToUnixTime()
                               };
            return postBlockDto;
        }
    }
}