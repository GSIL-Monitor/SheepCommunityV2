using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Paragraphs.Validators
{
    /// <summary>
    ///     显示一节的校验器。
    /// </summary>
    public class ParagraphShowValidator : AbstractValidator<ParagraphShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ParagraphShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ParagraphShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                     RuleFor(x => x.ParagraphNumber).NotEmpty().WithMessage(Resources.ParagraphNumberRequired);
                                 });
        }
    }
}