using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Paragraphs.Validators
{
    /// <summary>
    ///     搜索一组节的校验器。
    /// </summary>
    public class ParagraphSearchValidator : AbstractValidator<ParagraphSearch>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Number",
                                                              "ViewsCount",
                                                              "BookmarksCount",
                                                              "CommentsCount",
                                                              "LikesCount",
                                                              "RatingsCount",
                                                              "RatingsAverageValue",
                                                              "SharesCount"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ParagraphSearchValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ParagraphSearchValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}