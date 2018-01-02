using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.SecurityTokens.Validators
{
    /// <summary>
    ///     校验验证码的校验器。
    /// </summary>
    public class SecurityTokenVerifyValidator : AbstractValidator<SecurityTokenVerify>
    {
        public static readonly HashSet<string> Purposes = new HashSet<string>
                                                          {
                                                              "Login",
                                                              "Register",
                                                              "Bind",
                                                              "ResetPassword"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="SecurityTokenVerifyValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public SecurityTokenVerifyValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(x => string.Format(Resources.PhoneNumberRequired)).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(x => string.Format(Resources.PhoneNumberFormatMismatch));
                                      RuleFor(x => x.Purpose).NotEmpty().WithMessage(x => string.Format(Resources.PurposeRequired));
                                      RuleFor(x => x.Purpose).Must(purpose => Purposes.Contains(purpose)).WithMessage(x => string.Format(Resources.PurposeRangeMismatch, Purposes.Join(","))).When(x => !x.Purpose.IsNullOrEmpty());
                                      RuleFor(x => x.Token).NotEmpty().WithMessage(x => string.Format(Resources.SecurityTokenRequired)).Length(6).WithMessage(x => string.Format(Resources.SecurityTokenLengthMismatch, 6));
                                  });
        }
    }
}