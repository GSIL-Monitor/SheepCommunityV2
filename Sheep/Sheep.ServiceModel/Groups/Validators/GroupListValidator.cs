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

        public static readonly HashSet<string> AccountStatuses = new HashSet<string>
                                                                 {
                                                                     "Approved",
                                                                     "Banned",
                                                                     "Disapproved",
                                                                     "PendingDeletion"
                                                                 };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "DisplayName",
                                                              "FullName",
                                                              "RefId",
                                                              "JoinMode",
                                                              "AccountStatus",
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
                                     RuleFor(x => x.AccountStatus).Must(accountStatus => AccountStatuses.Contains(accountStatus)).WithMessage(Resources.AccountStatusRangeMismatch, AccountStatuses.Join(",")).When(x => !x.AccountStatus.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}