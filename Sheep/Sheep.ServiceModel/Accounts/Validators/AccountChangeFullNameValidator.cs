using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改真实姓名的校验器。
    /// </summary>
    public class AccountChangeFullNameValidator : AbstractValidator<AccountChangeFullName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeFullNameValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeFullNameValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.FullName).Length(2, 64).WithMessage(Resources.FullNameLengthMismatch, 2, 64).When(x => !x.FullName.IsNullOrEmpty());
                                 });
        }
    }
}