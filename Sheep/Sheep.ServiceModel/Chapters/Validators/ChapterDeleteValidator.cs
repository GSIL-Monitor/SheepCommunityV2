using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     删除一章的校验器。
    /// </summary>
    public class ChapterDeleteValidator : AbstractValidator<ChapterDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                        RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                        RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(x => string.Format(Resources.ChapterNumberRequired));
                                    });
        }
    }
}