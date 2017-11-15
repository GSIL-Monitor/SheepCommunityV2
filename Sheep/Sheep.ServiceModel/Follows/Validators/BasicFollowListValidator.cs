using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Follows.Validators
{
    /// <summary>
    ///     列举一组被关注者的关注基本信息的校验器。
    /// </summary>
    public class BasicFollowListOfFollowingUserValidator : AbstractValidator<BasicFollowListOfFollowingUser>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicFollowListOfFollowingUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicFollowListOfFollowingUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                 });
        }
    }

    /// <summary>
    ///     列举一组关注者的关注基本信息的校验器。
    /// </summary>
    public class BasicFollowListOfFollowerValidator : AbstractValidator<BasicFollowListOfFollower>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicFollowListOfFollowerValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicFollowListOfFollowerValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                 });
        }
    }
}