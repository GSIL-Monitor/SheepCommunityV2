using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Identities.Validators
{
    /// <summary>
    ///     使用用户名称或电子邮件地址及密码登录身份的校验器。
    /// </summary>
    public class IdentityLoginByCredentialsValidator : AbstractValidator<IdentityLoginByCredentials>
    {
        /// <summary>
        ///     初始化一个新的<see cref="IdentityLoginByCredentialsValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public IdentityLoginByCredentialsValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.UserNameOrEmail).NotEmpty().WithMessage(Resources.UserNameOrEmailRequired);
                                      RuleFor(x => x.Password).NotEmpty().WithMessage(Resources.PasswordRequired).Length(4, 256).WithMessage(Resources.PasswordLengthMismatch, 4);
                                  });
        }
    }

    /// <summary>
    ///     使用手机号码及验证码登录身份的校验器。
    /// </summary>
    public class IdentityLoginByMobileValidator : AbstractValidator<IdentityLoginByMobile>
    {
        /// <summary>
        ///     初始化一个新的<see cref="IdentityLoginByMobileValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public IdentityLoginByMobileValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(Resources.PhoneNumberRequired).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                                      RuleFor(x => x.Token).NotEmpty().WithMessage(Resources.SecurityTokenRequired).Length(6).WithMessage(Resources.SecurityTokenLengthMismatch, 6);
                                  });
        }
    }

    /// <summary>
    ///     使用微博帐号登录身份的校验器。
    /// </summary>
    public class IdentityLoginByWeiboValidator : AbstractValidator<IdentityLoginByWeibo>
    {
        /// <summary>
        ///     初始化一个新的<see cref="IdentityLoginByWeiboValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public IdentityLoginByWeiboValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }

    /// <summary>
    ///     使用微信帐号登录身份的校验器。
    /// </summary>
    public class IdentityLoginByWeixinValidator : AbstractValidator<IdentityLoginByWeixin>
    {
        /// <summary>
        ///     初始化一个新的<see cref="IdentityLoginByWeixinValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public IdentityLoginByWeixinValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }

    /// <summary>
    ///     使用QQ帐号登录身份的校验器。
    /// </summary>
    public class IdentityLoginByQQValidator : AbstractValidator<IdentityLoginByQQ>
    {
        /// <summary>
        ///     初始化一个新的<see cref="IdentityLoginByQQValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public IdentityLoginByQQValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }
}