using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Recommendations.Validators
{
    /// <summary>
    ///     创建一个推荐的校验器。
    /// </summary>
    public class RecommendationCreateValidator : AbstractValidator<RecommendationCreate>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "帖子"
                                                              };

        /// <summary>
        ///     初始化一个新的<see cref="RecommendationCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public RecommendationCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ContentType).NotEmpty().WithMessage(x => string.Format(Resources.ContentTypeRequired));
                                      RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ContentTypeRangeMismatch, ContentTypes.Join(","))).When(x => !x.ContentType.IsNullOrEmpty());
                                      RuleFor(x => x.ContentId).NotEmpty().WithMessage(x => string.Format(Resources.ContentIdRequired));
                                      RuleFor(x => x.Position).NotEmpty().WithMessage(x => string.Format(Resources.PositionRequired));
                                  });
        }
    }
}