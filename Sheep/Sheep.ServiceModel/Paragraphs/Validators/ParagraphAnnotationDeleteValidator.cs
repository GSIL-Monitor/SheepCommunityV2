using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Paragraphs.Validators
{
    /// <summary>
    ///     删除一条节注释的校验器。
    /// </summary>
    public class ParagraphAnnotationDeleteValidator : AbstractValidator<ParagraphAnnotationDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ParagraphAnnotationDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ParagraphAnnotationDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                        RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                        RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                        RuleFor(x => x.ParagraphNumber).NotEmpty().WithMessage(Resources.ParagraphNumberRequired);
                                        RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(Resources.AnnotationNumberRequired);
                                    });
        }
    }
}