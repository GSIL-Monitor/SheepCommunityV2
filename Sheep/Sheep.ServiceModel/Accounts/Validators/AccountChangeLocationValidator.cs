using System;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Model.Geo;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改所在地的校验器。
    /// </summary>
    public class AccountChangeLocationValidator : AbstractValidator<AccountChangeLocation>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeLocationValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeLocationValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Country).Must(CountriesContains).WithMessage(Resources.CountryRangeMismatch).When(x => !x.Country.IsNullOrEmpty());
                                 });
        }

        private bool CountriesContains(string country)
        {
            var countryRepo = HostContext.AppHost.Resolve<ICountryRepository>();
            using (countryRepo as IDisposable)
            {
                return countryRepo.GetCountryByName(country) != null;
            }
        }
    }
}