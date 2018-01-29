using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Recommendations.Validators
{
    /// <summary>
    ///     更新一个推荐的校验器。
    /// </summary>
    public class RecommendationUpdateValidator : AbstractValidator<RecommendationUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="RecommendationUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public RecommendationUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.RecommendationId).NotEmpty().WithMessage(x => string.Format(Resources.RecommendationIdRequired));
                                     RuleFor(x => x.Position).NotEmpty().WithMessage(x => string.Format(Resources.PositionRequired));
                                 });
        }
    }
}