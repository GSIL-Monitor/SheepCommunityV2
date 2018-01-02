using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Votes.Validators
{
    /// <summary>
    ///     显示一个投票的校验器。
    /// </summary>
    public class VoteShowValidator : AbstractValidator<VoteShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VoteShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VoteShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdRequired));
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                 });
        }
    }
}