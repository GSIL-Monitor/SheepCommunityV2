using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     显示一章的校验器。
    /// </summary>
    public class ChapterShowValidator : AbstractValidator<ChapterShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                 });
        }
    }
}