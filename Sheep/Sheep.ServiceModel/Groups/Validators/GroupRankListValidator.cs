using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     查询并列举一组群组排行的校验器。
    /// </summary>
    public class GroupRankListValidator : AbstractValidator<GroupRankList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "PostViewsCount",
                                                              "PostViewsRank",
                                                              "ParagraphViewsCount",
                                                              "ParagraphViewsRank"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="GroupRankListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupRankListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}