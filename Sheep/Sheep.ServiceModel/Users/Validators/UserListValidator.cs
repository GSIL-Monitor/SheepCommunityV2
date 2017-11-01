using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     列举一组用户的校验器。
    /// </summary>
    public class UserListValidator : AbstractValidator<UserList>
    {
        public static readonly HashSet<string> AccountStatuses = new HashSet<string>
                                                                 {
                                                                     "等待审核",
                                                                     "审核通过",
                                                                     "已查封",
                                                                     "审核失败",
                                                                     "等待删除"
                                                                 };

        /// <summary>
        ///     初始化一个新的<see cref="UserListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.AccountStatus).Must(accountStatus => AccountStatuses.Contains(accountStatus)).WithMessage(Resources.AccountStatusRangeMismatch, AccountStatuses.Join(",")).When(x => !x.AccountStatus.IsNullOrEmpty());
                                 });
        }
    }
}