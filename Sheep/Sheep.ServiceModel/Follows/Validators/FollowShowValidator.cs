using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.Follows.Validators
{
    /// <summary>
    ///     显示一个关注的校验器。
    /// </summary>
    public class FollowShowValidator : AbstractValidator<FollowShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FollowShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FollowShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     
                                 });
        }
    }
}