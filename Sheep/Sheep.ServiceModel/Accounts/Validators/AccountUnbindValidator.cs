using System;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Common.Auth;
using Sheep.Model.Auth.Providers;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     解除绑定手机号码及验证码帐户的校验器。
    /// </summary>
    public class AccountUnbindMobileValidator : AbstractValidator<AccountUnbindMobile>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountUnbindMobileValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountUnbindMobileValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(Resources.PhoneNumberRequired).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                                        RuleFor(x => x.PhoneNumber).Must(PhoneNumberExists).WithMessage(Resources.PhoneNumberNotExists).When(x => !x.PhoneNumber.IsNullOrEmpty());
                                    });
        }

        private bool PhoneNumberExists(string phoneNumber)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                if (authRepo is IUserAuthRepositoryExtended authRepoExtended)
                {
                    return authRepoExtended.GetUserAuthDetailsByProvider(MobileAuthProvider.Name, phoneNumber) != null;
                }
                return false;
            }
        }
    }

    /// <summary>
    ///     解除绑定微博帐号帐户的校验器。
    /// </summary>
    public class AccountUnbindWeiboValidator : AbstractValidator<AccountUnbindWeibo>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountUnbindWeiboValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountUnbindWeiboValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.WeiboUserId).NotEmpty().WithMessage(Resources.WeiboUserIdRequired);
                                        RuleFor(x => x.WeiboUserId).Must(WeiboUserIdExists).WithMessage(Resources.WeiboUserIdNotExists).When(x => !x.WeiboUserId.IsNullOrEmpty());
                                    });
        }

        private bool WeiboUserIdExists(string weiboUserId)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                if (authRepo is IUserAuthRepositoryExtended authRepoExtended)
                {
                    return authRepoExtended.GetUserAuthDetailsByProvider(WeiboAuthProvider.Name, weiboUserId) != null;
                }
                return false;
            }
        }
    }

    /// <summary>
    ///     解除绑定微信帐号帐户的校验器。
    /// </summary>
    public class AccountUnbindWeixinValidator : AbstractValidator<AccountUnbindWeixin>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountUnbindWeixinValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountUnbindWeixinValidator()
        {
            RuleSet(ApplyTo.Delete, () => { RuleFor(x => x.WeixinUserId).NotEmpty().WithMessage(Resources.WeixinUserIdRequired); });
        }
    }

    /// <summary>
    ///     解除绑定QQ帐号帐户的校验器。
    /// </summary>
    public class AccountUnbindQQValidator : AbstractValidator<AccountUnbindQQ>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountUnbindQQValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountUnbindQQValidator()
        {
            RuleSet(ApplyTo.Delete, () => { RuleFor(x => x.QQUserId).NotEmpty().WithMessage(Resources.QQUserIdRequired); });
        }
    }
}