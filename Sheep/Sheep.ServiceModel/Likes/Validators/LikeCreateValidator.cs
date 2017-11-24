using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Likes.Validators
{
    /// <summary>
    ///     新建一个点赞的校验器。
    /// </summary>
    public class LikeCreateValidator : AbstractValidator<LikeCreate>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "帖子"
                                                              };

        /// <summary>
        ///     初始化一个新的<see cref="LikeCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public LikeCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ContentType).NotEmpty().WithMessage(Resources.ContentTypeRequired);
                                      RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(Resources.ContentTypeRangeMismatch, ContentTypes.Join(","));
                                      RuleFor(x => x.ContentId).NotEmpty().WithMessage(Resources.ContentIdRequired);
                                  });
        }
    }
}