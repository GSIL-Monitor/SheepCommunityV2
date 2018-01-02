using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     删除一条章注释的校验器。
    /// </summary>
    public class ChapterAnnotationDeleteValidator : AbstractValidator<ChapterAnnotationDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterAnnotationDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterAnnotationDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                        RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                        RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(x => string.Format(Resources.ChapterNumberRequired));
                                        RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(x => string.Format(Resources.AnnotationNumberRequired));
                                    });
        }
    }
}