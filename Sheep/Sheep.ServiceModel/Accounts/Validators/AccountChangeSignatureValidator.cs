using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改签名的校验器。
    /// </summary>
    public class AccountChangeSignatureValidator : AbstractValidator<AccountChangeSignature>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeSignatureValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeSignatureValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Signature).Length(4, 128).WithMessage(x => string.Format(Resources.SignatureLengthMismatch, 4, 128)).When(x => !x.Signature.IsNullOrEmpty());
                                 });
        }
    }
}