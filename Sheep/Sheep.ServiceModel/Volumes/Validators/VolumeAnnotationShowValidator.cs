using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     显示一个卷注释的校验器。
    /// </summary>
    public class VolumeAnnotationShowValidator : AbstractValidator<VolumeAnnotationShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VolumeAnnotationShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeAnnotationShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(Resources.AnnotationNumberRequired);
                                 });
        }
    }
}