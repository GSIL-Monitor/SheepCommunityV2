using System;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     注册帐户的校验器。
    /// </summary>
    public class AccountRegisterValidator : AbstractValidator<AccountRegister>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountRegisterValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountRegisterValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.UserName).NotEmpty().WithMessage(x => string.Format(Resources.UserNameOrEmailRequired)).Length(4, 256).WithMessage(x => string.Format(Resources.UserNameLengthMismatch, 4, 256)).When(x => x.Email.IsNullOrEmpty());
                                      RuleFor(x => x.Email).NotEmpty().WithMessage(x => string.Format(Resources.UserNameOrEmailRequired)).Length(4, 256).WithMessage(x => string.Format(Resources.EmailLengthMismatch, 4, 256)).EmailAddress().WithMessage(x => string.Format(Resources.EmailFormatMismatch)).When(x => x.UserName.IsNullOrEmpty());
                                      RuleFor(x => x.UserName).Must(UserNameOrEmailNotExists).WithMessage(x => string.Format(Resources.UserNameAlreadyExists)).When(x => !x.UserName.IsNullOrEmpty());
                                      RuleFor(x => x.Email).Must(UserNameOrEmailNotExists).WithMessage(x => string.Format(Resources.EmailAlreadyExists)).When(x => !x.Email.IsNullOrEmpty());
                                      RuleFor(x => x.Password).NotEmpty().WithMessage(x => string.Format(Resources.PasswordRequired)).Length(4, 256).WithMessage(x => string.Format(Resources.PasswordLengthMismatch, 4, 256));
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
}