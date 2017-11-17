using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     列举一组群组的校验器。
    /// </summary>
    public class GroupListValidator : AbstractValidator<GroupList>
    {
        public static readonly HashSet<string> JoinModes = new HashSet<string>
                                                           {
                                                               "Direct",
                                                               "RequireVerification",
                                                               "Joinless"
                                                           };

        public static readonly HashSet<string> Statuses = new HashSet<string>
                                                                 {
                                                                     "待审核",
                                                                     "审核通过",
                                                                     "已禁止",
                                                                     "审核失败",
                                                                     "等待删除"
                                                                 };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "DisplayName",
                                                              "FullName",
                                                              "RefId",
                                                              "JoinMode",
                                                              "Status",
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "TotalMembers"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="GroupListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.JoinMode).Must(joinMode => JoinModes.Contains(joinMode)).WithMessage(Resources.JoinModeRangeMismatch, JoinModes.Join(",")).When(x => !x.JoinMode.IsNullOrEmpty());
                                     RuleFor(x => x.Status).Must(status => Statuses.Contains(status)).WithMessage(Resources.StatusRangeMismatch, Statuses.Join(",")).When(x => !x.Status.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}