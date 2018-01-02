using System;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改出生日期的校验器。
    /// </summary>
    public class AccountChangeBirthDateValidator : AbstractValidator<AccountChangeBirthDate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeBirthDateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeBirthDateValidator()
        {
            var minDate = new DateTime(1900, 1, 1, 0, 0, 0);
            var maxDate = DateTime.Today;
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BirthDate).InclusiveBetween(minDate, maxDate).WithMessage(x => string.Format(Resources.BirthDateRangeMismatch, minDate, maxDate)).When(x => x.BirthDate.HasValue);
                                 });
        }
    }
}