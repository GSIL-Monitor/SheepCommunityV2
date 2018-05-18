using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.PostBlocks.Validators
{
    /// <summary>
    ///     显示一个帖子屏蔽的校验器。
    /// </summary>
    public class PostBlockShowValidator : AbstractValidator<PostBlockShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="PostBlockShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostBlockShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                 });
        }
    }
}