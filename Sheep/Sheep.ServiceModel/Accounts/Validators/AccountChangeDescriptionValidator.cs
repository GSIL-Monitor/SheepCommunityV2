using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改简介的校验器。
    /// </summary>
    public class AccountChangeDescriptionValidator : AbstractValidator<AccountChangeDescription>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeDescriptionValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeDescriptionValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Description).Length(4, 8192).WithMessage(Resources.DescriptionLengthMismatch, 4, 8192).When(x => !x.Description.IsNullOrEmpty());
                                 });
        }
    }
}