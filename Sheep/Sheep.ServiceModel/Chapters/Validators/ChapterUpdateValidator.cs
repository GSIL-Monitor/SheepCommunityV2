using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     更新一章的校验器。
    /// </summary>
    public class ChapterUpdateValidator : AbstractValidator<ChapterUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                     RuleFor(x => x.Title).NotEmpty().WithMessage(Resources.TitleRequired);
                                 });
        }
    }
}