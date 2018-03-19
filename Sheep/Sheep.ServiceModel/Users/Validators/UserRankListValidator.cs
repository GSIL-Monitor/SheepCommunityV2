using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     查询并列举一组用户排行的校验器。
    /// </summary>
    public class UserRankListValidator : AbstractValidator<UserRankList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "PostViewsCount",
                                                              "PostViewsRank",
                                                              "ParagraphViewsCount",
                                                              "ParagraphViewsRank"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="UserRankListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserRankListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}