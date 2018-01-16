using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Job.ServiceModel.Properties;

namespace Sheep.Job.ServiceModel.Users.Validators
{
    /// <summary>
    ///     查询并计算一组用户声望的校验器。
    /// </summary>
    public class UserCalculateValidator : AbstractValidator<UserCalculate>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "UserName",
                                                              "Email",
                                                              "DisplayName",
                                                              "FullName",
                                                              "BirthDate",
                                                              "TimeZone",
                                                              "Language",
                                                              "Status",
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "Points",
                                                              "Reputation"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="UserCalculateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserCalculateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}