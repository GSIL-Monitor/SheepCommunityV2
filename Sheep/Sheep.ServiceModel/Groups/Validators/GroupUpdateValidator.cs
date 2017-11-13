using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Model.Geo;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     更新群组的校验器。
    /// </summary>
    public class GroupUpdateValidator : AbstractValidator<GroupUpdate>
    {
        public static readonly HashSet<string> JoinModes = new HashSet<string>
                                                           {
                                                               "Direct",
                                                               "RequireVerification",
                                                               "Joinless"
                                                           };

        /// <summary>
        ///     初始化一个新的<see cref="GroupUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(Resources.DisplayNameRequired);
                                     RuleFor(x => x.Country).Must(CountriesContains).WithMessage(Resources.CountryRangeMismatch).When(x => !x.Country.IsNullOrEmpty());
                                     RuleFor(x => x.JoinMode).Must(joinMode => JoinModes.Contains(joinMode)).WithMessage(Resources.JoinModeRangeMismatch, JoinModes.Join(",")).When(x => !x.JoinMode.IsNullOrEmpty());
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