using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Replies.Validators
{
    /// <summary>
    ///     更新一个回复的校验器。
    /// </summary>
    public class ReplyUpdateValidator : AbstractValidator<ReplyUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ReplyUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ReplyUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.ReplyId).NotEmpty().WithMessage(Resources.ReplyIdRequired);
                                     RuleFor(x => x.Content).NotEmpty().WithMessage(Resources.ContentRequired);
                                 });
        }
    }
}