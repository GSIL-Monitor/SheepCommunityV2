using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Books.Validators
{
    /// <summary>
    ///     删除一本书籍的校验器。
    /// </summary>
    public class BookDeleteValidator : AbstractValidator<BookDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BookDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                    });
        }
    }
}