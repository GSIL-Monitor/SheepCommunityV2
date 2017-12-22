using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Bookmarks.Validators
{
    /// <summary>
    ///     取消一个收藏的校验器。
    /// </summary>
    public class BookmarkDeleteValidator : AbstractValidator<BookmarkDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BookmarkDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookmarkDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                    });
        }
    }
}