using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     更新一条章注释的校验器。
    /// </summary>
    public class ChapterAnnotationUpdateValidator : AbstractValidator<ChapterAnnotationUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterAnnotationUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterAnnotationUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                     RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(Resources.AnnotationNumberRequired);
                                     RuleFor(x => x.Annotation).NotEmpty().WithMessage(Resources.AnnotationRequired);
                                 });
        }
    }
}