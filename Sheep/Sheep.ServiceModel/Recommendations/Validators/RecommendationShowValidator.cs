using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Recommendations.Validators
{
    /// <summary>
    ///     显示一个推荐的校验器。
    /// </summary>
    public class RecommendationShowValidator : AbstractValidator<RecommendationShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="RecommendationShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public RecommendationShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.RecommendationId).NotEmpty().WithMessage(x => string.Format(Resources.RecommendationIdRequired));
                                 });
        }
    }
}