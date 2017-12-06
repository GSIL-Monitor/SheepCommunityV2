using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     删除一个卷注释的校验器。
    /// </summary>
    public class VolumeAnnotationDeleteValidator : AbstractValidator<VolumeAnnotationDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VolumeAnnotationDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeAnnotationDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                        RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                        RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(Resources.AnnotationNumberRequired);
                                    });
        }
    }
}