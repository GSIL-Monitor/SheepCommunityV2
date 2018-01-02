using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改密码的校验器。
    /// </summary>
    public class AccountChangePasswordValidator : AbstractValidator<AccountChangePassword>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangePasswordValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangePasswordValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Password).NotEmpty().WithMessage(x => string.Format(Resources.PasswordRequired)).Length(4, 256).WithMessage(x => string.Format(Resources.PasswordLengthMismatch, 4, 256));
                                 });
        }
    }
}