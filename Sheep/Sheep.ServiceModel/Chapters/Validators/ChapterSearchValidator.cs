using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     搜索一组章的校验器。
    /// </summary>
    public class ChapterSearchValidator : AbstractValidator<ChapterSearch>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Number",
                                                              "ParagraphsCount",
                                                              "ViewsCount",
                                                              "BookmarksCount",
                                                              "CommentsCount",
                                                              "LikesCount",
                                                              "RatingsCount",
                                                              "RatingsAverageValue",
                                                              "SharesCount"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ChapterSearchValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterSearchValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}