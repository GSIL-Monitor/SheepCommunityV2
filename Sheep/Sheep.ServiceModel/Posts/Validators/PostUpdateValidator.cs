using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Posts.Validators
{
    /// <summary>
    ///     更新一个帖子的校验器。
    /// </summary>
    public class PostUpdateValidator : AbstractValidator<PostUpdate>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "图文",
                                                                  "音频",
                                                                  "视频"
                                                              };

        /// <summary>
        ///     初始化一个新的<see cref="PostUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.PostId).NotEmpty().WithMessage(x => string.Format(Resources.PostIdRequired));
                                     RuleFor(x => x.Title).NotEmpty().WithMessage(x => string.Format(Resources.TitleRequired));
                                     RuleFor(x => x.ContentType).NotEmpty().WithMessage(x => string.Format(Resources.ContentTypeRequired));
                                     RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ContentTypeRangeMismatch, ContentTypes.Join(","))).When(x => !x.ContentType.IsNullOrEmpty());
                                     RuleFor(x => x.Content).NotEmpty().WithMessage(x => string.Format(Resources.ContentRequired)).When(x => x.ContentType == "图文");
                                     RuleFor(x => x.ContentUrl).NotEmpty().WithMessage(x => string.Format(Resources.ContentUrlRequired)).When(x => x.ContentType == "音频" || x.ContentType == "视频");
                                 });
        }
    }
}