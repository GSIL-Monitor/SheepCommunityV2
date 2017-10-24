using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     使用用户名称或电子邮件地址及密码登录帐户的校验器。
    /// </summary>
    public class AccountLoginByCredentialsValidator : AbstractValidator<AccountLoginByCredentials>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountLoginByCredentialsValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountLoginByCredentialsValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.UserNameOrEmail).NotEmpty().WithMessage(Resources.UserNameOrEmailRequired);
                                      RuleFor(x => x.Password).NotEmpty().WithMessage(Resources.PasswordRequired).Length(4, 256).WithMessage(Resources.PasswordLengthMismatch, 4);
                                  });
        }
    }

    /// <summary>
    ///     使用手机号码及验证码登录帐户的校验器。
    /// </summary>
    public class AccountLoginByMobileValidator : AbstractValidator<AccountLoginByMobile>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountLoginByMobileValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountLoginByMobileValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(Resources.PhoneNumberRequired).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                                      RuleFor(x => x.Token).NotEmpty().WithMessage(Resources.SecurityTokenRequired).Length(6).WithMessage(Resources.SecurityTokenLengthMismatch, 6);
                                  });
        }
    }

    /// <summary>
    ///     使用微博帐号登录帐户的校验器。
    /// </summary>
    public class AccountLoginByWeiboValidator : AbstractValidator<AccountLoginByWeibo>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountLoginByWeiboValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountLoginByWeiboValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.OpenId).NotEmpty().WithMessage(Resources.OpenIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }

    /// <summary>
    ///     使用微信帐号登录帐户的校验器。
    /// </summary>
    public class AccountLoginByWeixinValidator : AbstractValidator<AccountLoginByWeixin>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountLoginByWeixinValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountLoginByWeixinValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.OpenId).NotEmpty().WithMessage(Resources.OpenIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }

    /// <summary>
    ///     使用QQ帐号登录帐户的校验器。
    /// </summary>
    public class AccountLoginByQQValidator : AbstractValidator<AccountLoginByQQ>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountLoginByQQValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountLoginByQQValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.OpenId).NotEmpty().WithMessage(Resources.OpenIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }
}