using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Paragraphs.Validators
{
    /// <summary>
    ///     更新一条节注释的校验器。
    /// </summary>
    public class ParagraphAnnotationUpdateValidator : AbstractValidator<ParagraphAnnotationUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ParagraphAnnotationUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ParagraphAnnotationUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(x => string.Format(Resources.ChapterNumberRequired));
                                     RuleFor(x => x.ParagraphNumber).NotEmpty().WithMessage(x => string.Format(Resources.ParagraphNumberRequired));
                                     RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(x => string.Format(Resources.AnnotationNumberRequired));
                                     RuleFor(x => x.Annotation).NotEmpty().WithMessage(x => string.Format(Resources.AnnotationRequired));
                                 });
        }
    }
}