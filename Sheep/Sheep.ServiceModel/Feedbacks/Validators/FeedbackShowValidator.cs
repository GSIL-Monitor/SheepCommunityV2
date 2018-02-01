using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Feedbacks.Validators
{
    /// <summary>
    ///     显示一个反馈的校验器。
    /// </summary>
    public class FeedbackShowValidator : AbstractValidator<FeedbackShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FeedbackShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.FeedbackId).NotEmpty().WithMessage(x => string.Format(Resources.FeedbackIdRequired));
                                 });
        }
    }
}