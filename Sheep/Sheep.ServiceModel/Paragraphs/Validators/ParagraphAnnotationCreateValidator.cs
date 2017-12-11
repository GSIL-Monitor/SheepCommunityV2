using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Paragraphs.Validators
{
    /// <summary>
    ///     创建一条节注释的校验器。
    /// </summary>
    public class ParagraphAnnotationCreateValidator : AbstractValidator<ParagraphAnnotationCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ParagraphAnnotationCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ParagraphAnnotationCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                      RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                      RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                      RuleFor(x => x.ParagraphNumber).NotEmpty().WithMessage(Resources.ParagraphNumberRequired);
                                      RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(Resources.AnnotationNumberRequired);
                                      RuleFor(x => x.Title).NotEmpty().WithMessage(Resources.TitleRequired);
                                      RuleFor(x => x.Annotation).NotEmpty().WithMessage(Resources.AnnotationRequired);
                                  });
        }
    }
}