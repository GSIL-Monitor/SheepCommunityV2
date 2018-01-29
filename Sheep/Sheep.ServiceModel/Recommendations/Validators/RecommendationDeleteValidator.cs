using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Recommendations.Validators
{
    /// <summary>
    ///     删除一个推荐的校验器。
    /// </summary>
    public class RecommendationDeleteValidator : AbstractValidator<RecommendationDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="RecommendationDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public RecommendationDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.RecommendationId).NotEmpty().WithMessage(x => string.Format(Resources.RecommendationIdRequired));
                                    });
        }
    }
}