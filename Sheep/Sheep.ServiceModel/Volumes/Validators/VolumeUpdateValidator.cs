using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     更新一卷的校验器。
    /// </summary>
    public class VolumeUpdateValidator : AbstractValidator<VolumeUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VolumeUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                     RuleFor(x => x.Title).NotEmpty().WithMessage(x => string.Format(Resources.TitleRequired));
                                 });
        }
    }
}