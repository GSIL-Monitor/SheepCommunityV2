using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Cities.Validators
{
    /// <summary>
    ///     显示一个城市的校验器。
    /// </summary>
    public class CityShowValidator : AbstractValidator<CityShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CityShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CityShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.CityId).NotEmpty().WithMessage(x => string.Format(Resources.CityIdRequired));
                                 });
        }
    }

    /// <summary>
    ///     根据城市名称显示一个城市的校验器。
    /// </summary>
    public class CityShowByNameValidator : AbstractValidator<CityShowByName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CityShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CityShowByNameValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.StateId).NotEmpty().WithMessage(x => string.Format(Resources.StateIdRequired));
                                     RuleFor(x => x.Name).NotEmpty().WithMessage(x => string.Format(Resources.NameRequired));
                                 });
        }
    }
}