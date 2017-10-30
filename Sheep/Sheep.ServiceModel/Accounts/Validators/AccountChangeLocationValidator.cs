using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改所在地的校验器。
    /// </summary>
    public class AccountChangeLocationValidator : AbstractValidator<AccountChangeLocation>
    {
        public static readonly HashSet<string> Countries = new HashSet<string>
                                                         {
                                                             "中国",
                                                         };

        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeLocationValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeLocationValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Country).Must(country => Countries.Contains(country)).WithMessage(Resources.CountryRangeMismatch, Countries.Join(",")).When(x => !x.Country.IsNullOrEmpty());
                                 });
        }
    }
}