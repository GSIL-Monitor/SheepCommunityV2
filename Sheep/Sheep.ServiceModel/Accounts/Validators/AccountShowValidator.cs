using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     显示一个用户的校验器。
    /// </summary>
    public class AccountShowValidator : AbstractValidator<AccountShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountShowValidator()
        {
            RuleSet(ApplyTo.Get, () => { });
        }
    }
}