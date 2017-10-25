using System;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Common.Auth;
using Sheep.Model.Auth.Providers;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     绑定用户名称或电子邮件地址及密码帐户的校验器。
    /// </summary>
    public class AccountBindCredentialsValidator : AbstractValidator<AccountBindCredentials>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountBindCredentialsValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountBindCredentialsValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.UserName).NotEmpty().WithMessage(Resources.UserNameOrEmailRequired).Length(4, 256).WithMessage(Resources.UserNameLengthMismatch, 4).When(x => x.Email.IsNullOrEmpty());
                                      RuleFor(x => x.Email).NotEmpty().WithMessage(Resources.UserNameOrEmailRequired).Length(4, 256).WithMessage(Resources.EmailLengthMismatch, 4).EmailAddress().WithMessage(Resources.EmailFormatMismatch).When(x => x.UserName.IsNullOrEmpty());
                                      RuleFor(x => x.UserName).Must(UserNameOrEmailNotExists).WithMessage(Resources.UserNameAlreadyExists).When(x => !x.UserName.IsNullOrEmpty());
                                      RuleFor(x => x.Email).Must(UserNameOrEmailNotExists).WithMessage(Resources.EmailAlreadyExists).When(x => !x.Email.IsNullOrEmpty());
                                      RuleFor(x => x.Password).NotEmpty().WithMessage(Resources.PasswordRequired).Length(4, 256).WithMessage(Resources.PasswordLengthMismatch, 4);
                                  });
        }

        private bool UserNameOrEmailNotExists(string userNameOrEmail)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                return authRepo.GetUserAuthByUserName(userNameOrEmail) == null;
            }
        }
    }

    /// <summary>
    ///     绑定手机号码及验证码帐户的校验器。
    /// </summary>
    public class AccountBindMobileValidator : AbstractValidator<AccountBindMobile>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountBindMobileValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountBindMobileValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(Resources.PhoneNumberRequired).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                                      RuleFor(x => x.PhoneNumber).Must(PhoneNumberNotExists).WithMessage(Resources.PhoneNumberAlreadyExists).When(x => !x.PhoneNumber.IsNullOrEmpty());
                                      RuleFor(x => x.Token).NotEmpty().WithMessage(Resources.SecurityTokenRequired).Length(6).WithMessage(Resources.SecurityTokenLengthMismatch, 6);
                                  });
        }

        private bool PhoneNumberNotExists(string phoneNumber)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                if (authRepo is IUserAuthRepositoryExtended authRepoExtended)
                {
                    return authRepoExtended.GetUserAuthDetailsByProvider(MobileAuthProvider.Name, phoneNumber) == null;
                }
                return false;
            }
        }
    }

    /// <summary>
    ///     绑定微博帐号帐户的校验器。
    /// </summary>
    public class AccountBindWeiboValidator : AbstractValidator<AccountBindWeibo>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountBindWeiboValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountBindWeiboValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.WeiboUserId).NotEmpty().WithMessage(Resources.WeiboUserIdRequired);
                                      RuleFor(x => x.WeiboUserId).Must(WeiboUserIdNotExists).WithMessage(Resources.WeiboUserIdAlreadyExists).When(x => !x.WeiboUserId.IsNullOrEmpty());
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }

        private bool WeiboUserIdNotExists(string weiboUserId)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                if (authRepo is IUserAuthRepositoryExtended authRepoExtended)
                {
                    return authRepoExtended.GetUserAuthDetailsByProvider(WeiboAuthProvider.Name, weiboUserId) == null;
                }
                return false;
            }
        }
    }

    /// <summary>
    ///     绑定微信帐号帐户的校验器。
    /// </summary>
    public class AccountBindWeixinValidator : AbstractValidator<AccountBindWeixin>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountBindWeixinValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountBindWeixinValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.WeixinUserId).NotEmpty().WithMessage(Resources.WeixinUserIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }

    /// <summary>
    ///     绑定QQ帐号帐户的校验器。
    /// </summary>
    public class AccountBindQQValidator : AbstractValidator<AccountBindQQ>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountBindQQValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountBindQQValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.QQUserId).NotEmpty().WithMessage(Resources.QQUserIdRequired);
                                      RuleFor(x => x.AccessToken).NotEmpty().WithMessage(Resources.AccessTokenRequired);
                                  });
        }
    }
}