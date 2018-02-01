using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Feedbacks.Validators
{
    /// <summary>
    ///     创建一个反馈的校验器。
    /// </summary>
    public class FeedbackCreateValidator : AbstractValidator<FeedbackCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FeedbackCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.Content).NotEmpty().WithMessage(x => string.Format(Resources.ContentRequired));
                                  });
        }
    }
}