using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     显示一个卷的校验器。
    /// </summary>
    public class VolumeShowValidator : AbstractValidator<VolumeShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VolumeShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                 });
        }
    }
}