using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     创建一章的校验器。
    /// </summary>
    public class ChapterCreateValidator : AbstractValidator<ChapterCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                      RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                      RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(x => string.Format(Resources.ChapterNumberRequired));
                                      RuleFor(x => x.Title).NotEmpty().WithMessage(x => string.Format(Resources.TitleRequired));
                                  });
        }
    }
}