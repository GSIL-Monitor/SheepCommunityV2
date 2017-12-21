using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Bookmarks.Validators
{
    /// <summary>
    ///     显示一个收藏的校验器。
    /// </summary>
    public class BookmarkShowValidator : AbstractValidator<BookmarkShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BookmarkShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookmarkShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                 });
        }
    }
}