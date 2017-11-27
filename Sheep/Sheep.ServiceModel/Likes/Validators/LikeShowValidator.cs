using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Likes.Validators
{
    /// <summary>
    ///     显示一个点赞的校验器。
    /// </summary>
    public class LikeShowValidator : AbstractValidator<LikeShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="LikeShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public LikeShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                 });
        }
    }
}