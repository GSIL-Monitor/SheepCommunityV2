using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Votes.Validators
{
    /// <summary>
    ///     取消一个投票的校验器。
    /// </summary>
    public class VoteDeleteValidator : AbstractValidator<VoteDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VoteDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VoteDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.ParentId).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdRequired));
                                    });
        }
    }
}