using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.States.Validators
{
    /// <summary>
    ///     显示一个省份的校验器。
    /// </summary>
    public class StateShowValidator : AbstractValidator<StateShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="StateShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public StateShowValidator()
        {
            RuleSet(ApplyTo.Get, () => { RuleFor(x => x.StateId).NotEmpty().WithMessage(x => string.Format(Resources.StateIdRequired)); });
        }
    }

    /// <summary>
    ///     根据省份名称显示一个省份的校验器。
    /// </summary>
    public class StateShowByNameValidator : AbstractValidator<StateShowByName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="StateShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public StateShowByNameValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.CountryId).NotEmpty().WithMessage(x => string.Format(Resources.CountryIdRequired));
                                     RuleFor(x => x.Name).NotEmpty().WithMessage(x => string.Format(Resources.NameRequired));
                                 });
        }
    }
}