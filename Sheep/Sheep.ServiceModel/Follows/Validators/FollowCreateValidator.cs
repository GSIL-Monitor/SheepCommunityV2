using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.Follows.Validators
{
    /// <summary>
    ///     新建一个关注的校验器。
    /// </summary>
    public class FollowCreateValidator : AbstractValidator<FollowCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FollowCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FollowCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      
                                  });
        }
    }
}