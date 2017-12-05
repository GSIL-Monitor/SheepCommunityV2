using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Books.Validators
{
    /// <summary>
    ///     创建一个书籍的校验器。
    /// </summary>
    public class BookCreateValidator : AbstractValidator<BookCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BookCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                      RuleFor(x => x.Title).NotEmpty().WithMessage(Resources.TitleRequired);
                                  });
        }
    }
}