using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     显示一个阅读的校验器。
    /// </summary>
    public class ViewShowValidator : AbstractValidator<ViewShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ViewShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ViewId).NotEmpty().WithMessage(x => string.Format(Resources.ViewIdRequired));
                                 });
        }
    }
}