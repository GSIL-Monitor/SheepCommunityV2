using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Follows.Validators
{
    /// <summary>
    ///     列举一组被关注者的关注的校验器。
    /// </summary>
    public class FollowListOfFollowingUserValidator : AbstractValidator<FollowListOfFollowingUser>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "IsBidirectional",
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="FollowListOfFollowingUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FollowListOfFollowingUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     列举一组关注者的关注的校验器。
    /// </summary>
    public class FollowListOfFollowerValidator : AbstractValidator<FollowListOfFollower>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "IsBidirectional",
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="FollowListOfFollowerValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FollowListOfFollowerValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}