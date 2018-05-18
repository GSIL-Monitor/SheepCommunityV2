using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.PostBlocks.Validators
{
    /// <summary>
    ///     取消一个帖子屏蔽的校验器。
    /// </summary>
    public class PostBlockDeleteValidator : AbstractValidator<PostBlockDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="PostBlockDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostBlockDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                    });
        }
    }
}