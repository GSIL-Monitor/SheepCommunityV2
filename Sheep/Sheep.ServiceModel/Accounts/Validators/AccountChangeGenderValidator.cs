using System.Collections.Generic;
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
        public static readonly HashSet<string> Genders = new HashSet<string>
                                                         {
                                                             "男",
                                                             "女"
                                                         };

        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeGenderValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeGenderValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Gender).Must(gender => Genders.Contains(gender)).WithMessage(Resources.GenderRangeMismatch, Genders.Join(",")).When(x => !x.Gender.IsNullOrEmpty());
                                 });
        }
    }
}