using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Posts.Validators
{
    /// <summary>
    ///     查询并列举一组帖子的校验器。
    /// </summary>
    public class PostListValidator : AbstractValidator<PostList>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "图文",
                                                                  "音频",
                                                                  "视频"
                                                              };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Title",
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "PublishedDate",
                                                              "ViewsCount",
                                                              "BookmarksCount",
                                                              "CommentsCount",
                                                              "LikesCount",
                                                              "RatingsCount",
                                                              "RatingsAverageValue",
                                                              "SharesCount",
                                                              "AbuseReportsCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="PostListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(Resources.ParentTypeRangeMismatch, ContentTypes.Join(",")).When(x => !x.ContentType.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}