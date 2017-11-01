using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Countries.Validators
{
    /// <summary>
    ///     显示一个国家的校验器。
    /// </summary>
    public class CountryShowValidator : AbstractValidator<CountryShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CountryShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CountryShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.CountryId).NotEmpty().WithMessage(Resources.CountryIdRequired);
                                 });
        }
    }

    /// <summary>
    ///     根据国家名称显示一个国家的校验器。
    /// </summary>
    public class CountryShowByNameValidator : AbstractValidator<CountryShowByName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CountryShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CountryShowByNameValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.Name).NotEmpty().WithMessage(Resources.NameRequired);
                                 });
        }
    }
}