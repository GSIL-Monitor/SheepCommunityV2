using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Feedbacks.Validators
{
    /// <summary>
    ///     更新一个反馈的校验器。
    /// </summary>
    public class FeedbackUpdateValidator : AbstractValidator<FeedbackUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FeedbackUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.FeedbackId).NotEmpty().WithMessage(x => string.Format(Resources.FeedbackIdRequired));
                                     RuleFor(x => x.Content).NotEmpty().WithMessage(x => string.Format(Resources.ContentRequired));
                                 });
        }
    }
}