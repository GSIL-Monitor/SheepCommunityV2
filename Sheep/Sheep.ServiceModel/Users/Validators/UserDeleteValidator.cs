using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     删除一个用户的校验器。
    /// </summary>
    public class UserDeleteValidator : AbstractValidator<UserDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="UserDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                    });
        }
    }
}