using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Validation;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Posts;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     删除一个帖子服务接口。
    /// </summary>
    public class DeletePostService : ChangePostService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(DeletePostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置删除一个帖子的校验器。
        /// </summary>
        public IValidator<PostDelete> PostDeleteValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 删除一个帖子

        /// <summary>
        ///     删除一个帖子。
        /// </summary>
        public async Task<object> Delete(PostDelete request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            {
                PostDeleteValidator.ValidateAndThrow(request, ApplyTo.Delete);
            }
            var existingPost = await PostRepo.GetPostAsync(request.PostId);
            if (existingPost == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostNotFound, request.PostId));
            }
            var authorId = GetSession().UserAuthId.ToInt(0);
            if (existingPost.AuthorId != authorId)
            {
                throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            }
            await PostRepo.DeletePostAsync(request.PostId);
            ResetCache(existingPost);
            //await NimClient.PostAsync(new FriendDeleteRequest
            //                          {
            //                              AccountId = authorId.ToString(),
            //                              FriendAccountId = request.PostId.ToString()
            //                          });
            return new PostDeleteResponse();
        }

        #endregion
    }
}