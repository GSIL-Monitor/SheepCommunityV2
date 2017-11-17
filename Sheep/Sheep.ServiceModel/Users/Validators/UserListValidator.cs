using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     查询并列举一组用户的校验器。
    /// </summary>
    public class UserListValidator : AbstractValidator<UserList>
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
                                                              "Points"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="UserListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}