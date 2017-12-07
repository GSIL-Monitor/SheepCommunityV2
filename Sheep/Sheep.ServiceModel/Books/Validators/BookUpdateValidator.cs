using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Books.Validators
{
    /// <summary>
    ///     更新一本书籍的校验器。
    /// </summary>
    public class BookUpdateValidator : AbstractValidator<BookUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BookUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.Title).NotEmpty().WithMessage(Resources.TitleRequired);
                                 });
        }
    }
}