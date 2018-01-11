using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Job.ServiceInterface.Properties;
using Sheep.Job.ServiceModel.Posts;
using Sheep.Model.Content;

namespace Sheep.Job.ServiceInterface.Posts
{
    /// <summary>
    ///     计算一组帖子分数服务接口。
    /// </summary>
    public class CalculatePostService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CalculatePostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置计算一组帖子分数的校验器。
        /// </summary>
        public IValidator<PostCalculate> PostCalculateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        #endregion

        #region 计算一组帖子分数

        /// <summary>
        ///     计算一组帖子分数。
        /// </summary>
        public async Task<object> Put(PostCalculate request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostCalculateValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingPosts = await PostRepo.FindPostsAsync(request.TitleFilter, request.Tag, request.ContentType, request.CreatedSince, request.ModifiedSince, request.PublishedSince, request.IsPublished ?? true, request.IsFeatured, "审核通过", request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingPosts == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostsNotFound));
            }
            foreach (var existingPost in existingPosts)
            {
                await PostRepo.UpdatePostContentQualityAsync(existingPost.Id, PostRepo.CalculatePostContentQuality(existingPost));
            }
            return new PostCalculateResponse();
        }

        #endregion
    }
}