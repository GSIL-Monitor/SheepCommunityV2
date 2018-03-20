using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     查询并列举一组群组的校验器。
    /// </summary>
    public class GroupListValidator : AbstractValidator<GroupList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "DisplayName",
                                                              "FullName",
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="GroupListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据编号列表查询并列举一组群组的校验器。
    /// </summary>
    public class GroupListByIdsValidator : AbstractValidator<GroupListByIds>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "DisplayName",
                                                              "FullName",
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="GroupListByIdsValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupListByIdsValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.GroupIds).NotEmpty().WithMessage(x => string.Format(Resources.GroupIdsRequired));
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}