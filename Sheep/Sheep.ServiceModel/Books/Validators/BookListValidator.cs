using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Books.Validators
{
    /// <summary>
    ///     查询并列举一组书籍的校验器。
    /// </summary>
    public class BookListValidator : AbstractValidator<BookList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "PublishedDate",
                                                              "VolumesCount",
                                                              "BookmarksCount",
                                                              "RatingsCount",
                                                              "RatingsAverageValue",
                                                              "SharesCount"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="BookListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}