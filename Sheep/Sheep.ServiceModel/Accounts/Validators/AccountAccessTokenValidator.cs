using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     获取微博授权码的校验器。
    /// </summary>
    public class AccountAccessTokenForWeiboValidator : AbstractValidator<AccountAccessTokenForWeibo>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountAccessTokenForWeiboValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountAccessTokenForWeiboValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.Code).NotEmpty().WithMessage(Resources.CodeRequired);
                                 });
        }
    }
}