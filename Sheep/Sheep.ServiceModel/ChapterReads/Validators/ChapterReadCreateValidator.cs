using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.ChapterReads.Validators
{
    /// <summary>
    ///     新建一个阅读的校验器。
    /// </summary>
    public class ChapterReadCreateValidator : AbstractValidator<ChapterReadCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterReadCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterReadCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ChapterId).NotEmpty().WithMessage(x => string.Format(Resources.ChapterIdRequired));
                                  });
        }
    }
}