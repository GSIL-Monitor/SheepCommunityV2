using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Content;
using Sheep.ServiceInterface.PostBlocks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.PostBlocks;

namespace Sheep.ServiceInterface.PostBlocks
{
    /// <summary>
    ///     根据上级列举一组帖子屏蔽信息服务接口。
    /// </summary>
    [CompressResponse]
    public class ListPostBlockByPostService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListPostBlockByPostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组帖子屏蔽的校验器。
        /// </summary>
        public IValidator<PostBlockListByPost> PostBlockListByPostValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子屏蔽的存储库。
        /// </summary>
        public IPostBlockRepository PostBlockRepo { get; set; }

        #endregion

        #region 列举一组帖子屏蔽

        /// <summary>
        ///     列举一组帖子屏蔽。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(PostBlockListByPost request)
        {
            if (request.IsMine.HasValue && request.IsMine.Value && !IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostBlockListByPostValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var blockerId = GetSession().UserAuthId.ToInt(0);
            var existingPostBlocks = await PostBlockRepo.FindPostBlocksByPostAsync(request.PostId, request.IsMine.HasValue && request.IsMine.Value ? blockerId : (int?) null, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingPostBlocks == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostBlocksNotFound));
            }
            var postsMap = (await PostRepo.GetPostsAsync(existingPostBlocks.Select(postBlock => postBlock.PostId).Distinct().ToList())).ToDictionary(post => post.Id, post => post);
            var postAuthorsMap = (await ((IUserAuthRepositoryExtended)AuthRepo).GetUserAuthsAsync(postsMap.Select(postPair => postPair.Value.AuthorId.ToString()).Distinct().ToList())).ToDictionary(postAuthAuth => postAuthAuth.Id, postAuthAuth => postAuthAuth);
            var blockersMap = (await ((IUserAuthRepositoryExtended)AuthRepo).GetUserAuthsAsync(existingPostBlocks.Select(postBlock => postBlock.BlockerId.ToString()).Distinct().ToList())).ToDictionary(blockerAuth => blockerAuth.Id, blockerAuth => blockerAuth);
            var postBlocksDto = existingPostBlocks.Select(postBlock =>
                                                          {
                                                              var post = postsMap.GetValueOrDefault(postBlock.PostId);
                                                              var postAuthor = postAuthorsMap.GetValueOrDefault(post.AuthorId);
                                                              var blocker = blockersMap.GetValueOrDefault(postBlock.BlockerId);
                                                              return postBlock.MapToPostBlockDto(post, postAuthor, blocker);
                                                          })
                                                  .ToList();
            return new PostBlockListResponse
                   {
                       PostBlocks = postBlocksDto
                   };
        }

        #endregion
    }
}