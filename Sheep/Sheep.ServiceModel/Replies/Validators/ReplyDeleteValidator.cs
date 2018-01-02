using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Replies.Validators
{
    /// <summary>
    ///     删除一个回复的校验器。
    /// </summary>
    public class ReplyDeleteValidator : AbstractValidator<ReplyDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ReplyDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ReplyDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.ReplyId).NotEmpty().WithMessage(x => string.Format(Resources.ReplyIdRequired));
                                    });
        }
    }
}