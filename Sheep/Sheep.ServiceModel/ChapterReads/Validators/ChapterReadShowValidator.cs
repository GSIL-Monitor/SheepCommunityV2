using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.ChapterReads.Validators
{
    /// <summary>
    ///     显示一个阅读的校验器。
    /// </summary>
    public class ChapterReadShowValidator : AbstractValidator<ChapterReadShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterReadShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterReadShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ChapterReadId).NotEmpty().WithMessage(x => string.Format(Resources.ChapterReadIdRequired));
                                 });
        }
    }

    /// <summary>
    ///     显示最后一个阅读的校验器。
    /// </summary>
    public class ChapterReadShowLastValidator : AbstractValidator<ChapterReadShowLast>
    {
        /// <summary>
        ///     初始化一个新的<see cref="Sheep.ServiceModel.ChapterReads.Validators.ChapterReadShowLastValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterReadShowLastValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                 });
        }
    }

    /// <summary>
    ///     根据章显示最后一个阅读的校验器。
    /// </summary>
    public class ChapterReadShowLastByChapterValidator : AbstractValidator<ChapterReadShowLastByChapter>
    {
        /// <summary>
        ///     初始化一个新的<see cref="Sheep.ServiceModel.ChapterReads.Validators.ChapterReadShowLastByChapterValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterReadShowLastByChapterValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ChapterId).NotEmpty().WithMessage(x => string.Format(Resources.ChapterIdRequired));
                                 });
        }
    }
}