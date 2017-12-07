using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     删除一卷的校验器。
    /// </summary>
    public class VolumeDeleteValidator : AbstractValidator<VolumeDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VolumeDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                        RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                    });
        }
    }
}