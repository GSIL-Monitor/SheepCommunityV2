using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Books.Validators
{
    /// <summary>
    ///     显示一本书籍的校验器。
    /// </summary>
    public class BookShowValidator : AbstractValidator<BookShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BookShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                 });
        }
    }
}