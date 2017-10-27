using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     更新用户的校验器。
    /// </summary>
    public class UserUpdateValidator : AbstractValidator<UserUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="UserUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(Resources.DisplayNameRequired);
                                     RuleFor(x => x.PrimaryEmail).EmailAddress().WithMessage(Resources.EmailFormatMismatch);
                                     RuleFor(x => x.PhoneNumber).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                                 });
        }
    }
}