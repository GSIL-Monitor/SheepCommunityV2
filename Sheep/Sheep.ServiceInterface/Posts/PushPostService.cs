using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JPush.Push;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Content;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Posts;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     推送一个帖子服务接口。
    /// </summary>
    public class PushPostService : ChangePostService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(PushPostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public IPushClient PushClient { get; set; }

        /// <summary>
        ///     获取及设置推送一个帖子的校验器。
        /// </summary>
        public IValidator<PostPush> PostPushValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 推送一个帖子

        /// <summary>
        ///     推送一个帖子。
        /// </summary>
        public async Task<object> Post(PostPush request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostPushValidator.ValidateAndThrow(request, ApplyTo.Push);
            //}
            var existingPost = await PostRepo.GetPostAsync(request.PostId);
            if (existingPost == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostNotFound, request.PostId));
            }
            //var authorId = GetSession().UserAuthId.ToInt(0);
            //if (existingPost.AuthorId != authorId)
            //{
            //    throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            //}
            var cidResponse = await PushClient.GetAsync(new CidRequest
                                                        {
                                                            Count = 1
                                                        });
            if (cidResponse.Cids == null || cidResponse.Cids.Count <= 0)
            {
                throw new HttpError(HttpStatusCode.InternalServerError);
            }
            await PushClient.PostAsync(new PushRequest
                                       {
                                           CId = cidResponse.Cids.FirstNonDefault(),
                                           Platform = new[]
                                                      {
                                                          "android",
                                                          "ios"
                                                      },
                                           Audience = "all",
                                           Notification = new Notification
                                                          {
                                                              Android = new NotificationAndroid
                                                                        {
                                                                            Alert = existingPost.Title,
                                                                            AlertType = 2 | 4,
                                                                            Extras = new Dictionary<string, object>
                                                                                     {
                                                                                         {
                                                                                             "PostId", existingPost.Id
                                                                                         }
                                                                                     }
                                                                        },
                                                              IOS = new NotificationIOS
                                                                    {
                                                                        Alert = existingPost.Title,
                                                                        Extras = new Dictionary<string, object>
                                                                                 {
                                                                                     {
                                                                                         "PostId", existingPost.Id
                                                                                     }
                                                                                 }
                                                                    }
                                                          },
                                           Options = new Options
                                                     {
                                                         TimeToLive = 86400 * 5,
                                                         IsApnsProduction = true,
                                                         ApnsCollapseId = string.Format("Post{0}", existingPost.Id)
                                                     }
                                       });
            return new PostPushResponse();
        }

        #endregion
    }
}