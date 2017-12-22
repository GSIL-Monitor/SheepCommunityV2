using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     取消一个查看的校验器。
    /// </summary>
    public class ViewDeleteValidator : AbstractValidator<ViewDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ViewDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.ViewId).NotEmpty().WithMessage(Resources.ViewIdRequired);
                                    });
        }
    }
}