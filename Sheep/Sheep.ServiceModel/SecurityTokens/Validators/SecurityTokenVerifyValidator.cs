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
                                      RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(Resources.PhoneNumberRequired).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                                      RuleFor(x => x.Purpose).NotEmpty().WithMessage(Resources.PurposeRequired);
                                      RuleFor(x => x.Purpose).Must(purpose => Purposes.Contains(purpose)).WithMessage(Resources.PurposeRangeMismatch, Purposes.Join(",")).When(x => !x.Purpose.IsNullOrEmpty());
                                      RuleFor(x => x.Token).NotEmpty().WithMessage(Resources.SecurityTokenRequired).Length(6).WithMessage(Resources.SecurityTokenLengthMismatch, 6);
                                  });
        }
    }
}