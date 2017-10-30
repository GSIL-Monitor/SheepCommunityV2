using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改性别的校验器。
    /// </summary>
    public class AccountChangeGenderValidator : AbstractValidator<AccountChangeGender>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeGenderValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeGenderValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Gender).InclusiveBetween(0, 1).WithMessage(Resources.GenderRangeMismatch, 0, 1).When(x => x.Gender.HasValue);
                                 });
        }
    }
}