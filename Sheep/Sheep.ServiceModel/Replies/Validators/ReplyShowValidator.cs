using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Replies.Validators
{
    /// <summary>
    ///     显示一个回复的校验器。
    /// </summary>
    public class ReplyShowValidator : AbstractValidator<ReplyShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ReplyShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ReplyShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ReplyId).NotEmpty().WithMessage(x => string.Format(Resources.ReplyIdRequired));
                                 });
        }
    }
}