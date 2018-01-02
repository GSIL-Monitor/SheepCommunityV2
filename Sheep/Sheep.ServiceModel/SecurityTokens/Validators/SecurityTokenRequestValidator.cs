using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.SecurityTokens.Validators
{
    /// <summary>
    ///     请求发送验证码的校验器。
    /// </summary>
    public class SecurityTokenRequestValidator : AbstractValidator<SecurityTokenRequest>
    {
        public static readonly HashSet<string> Purposes = new HashSet<string>
                                                          {
                                                              "Login",
                                                              "Register",
                                                              "Bind",
                                                              "ResetPassword"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="SecurityTokenRequestValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public SecurityTokenRequestValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(x => string.Format(Resources.PhoneNumberRequired)).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(x => string.Format(Resources.PhoneNumberFormatMismatch));
                                      RuleFor(x => x.Purpose).NotEmpty().WithMessage(x => string.Format(Resources.PurposeRequired));
                                      RuleFor(x => x.Purpose).Must(purpose => Purposes.Contains(purpose)).WithMessage(x => string.Format(Resources.PurposeRangeMismatch, Purposes.Join(","))).When(x => !x.Purpose.IsNullOrEmpty());
                                  });
        }
    }
}