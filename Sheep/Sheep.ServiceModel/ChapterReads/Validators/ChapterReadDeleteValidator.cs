using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.ChapterReads.Validators
{
    /// <summary>
    ///     取消一个阅读的校验器。
    /// </summary>
    public class ChapterReadDeleteValidator : AbstractValidator<ChapterReadDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterReadDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterReadDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.ChapterReadId).NotEmpty().WithMessage(x => string.Format(Resources.ChapterReadIdRequired));
                                    });
        }
    }
}