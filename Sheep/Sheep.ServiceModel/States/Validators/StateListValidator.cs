using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.States.Validators
{
    /// <summary>
    ///     列举一组省份的校验器。
    /// </summary>
    public class StateListValidator : AbstractValidator<StateList>
    {
        /// <summary>
        ///     初始化一个新的<see cref="StateListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public StateListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.CountryId).NotEmpty().WithMessage(x => string.Format(Resources.CountryIdRequired));
                                 });
        }
    }
}