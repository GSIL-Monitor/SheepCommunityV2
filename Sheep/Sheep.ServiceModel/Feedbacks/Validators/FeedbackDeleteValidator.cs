using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Feedbacks.Validators
{
    /// <summary>
    ///     删除一个反馈的校验器。
    /// </summary>
    public class FeedbackDeleteValidator : AbstractValidator<FeedbackDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FeedbackDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.FeedbackId).NotEmpty().WithMessage(x => string.Format(Resources.FeedbackIdRequired));
                                    });
        }
    }
}