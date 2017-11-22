using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.Follows.Validators
{
    /// <summary>
    ///     取消一个关注的校验器。
    /// </summary>
    public class FollowDeleteValidator : AbstractValidator<FollowDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FollowDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FollowDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        
                                    });
        }
    }
}