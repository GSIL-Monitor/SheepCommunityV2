using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     显示一个用户排行的校验器。
    /// </summary>
    public class UserRankShowValidator : AbstractValidator<UserRankShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="UserRankShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserRankShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                 });
        }
    }
}