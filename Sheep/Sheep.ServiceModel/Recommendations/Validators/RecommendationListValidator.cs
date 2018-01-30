using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Recommendations.Validators
{
    /// <summary>
    ///     查询并列举一组推荐的校验器。
    /// </summary>
    public class RecommendationListValidator : AbstractValidator<RecommendationList>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "帖子"
                                                              };

        /// <summary>
        ///     初始化一个新的<see cref="RecommendationListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public RecommendationListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ContentTypeRangeMismatch, ContentTypes.Join(","))).When(x => !x.ContentType.IsNullOrEmpty());
                                 });
        }
    }
}