using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Cities.Validators
{
    /// <summary>
    ///     列举一组城市的校验器。
    /// </summary>
    public class CityListValidator : AbstractValidator<CityList>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CityListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CityListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.StateId).NotEmpty().WithMessage(Resources.StateIdRequired);
                                 });
        }
    }
}