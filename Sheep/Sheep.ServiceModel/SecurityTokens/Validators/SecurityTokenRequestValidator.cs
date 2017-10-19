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
        /// <summary>
        ///     初始化一个新的<see cref="SecurityTokenRequestValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public SecurityTokenRequestValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(Resources.PhoneNumberRequired).Matches("^1[3|4|5|7|8][0-9]{9}$").WithMessage(Resources.PhoneNumberFormatMismatch);
                                      RuleFor(x => x.Purpose).NotEmpty().WithMessage(Resources.PurposeRequired);
                                  });
        }
    }
}