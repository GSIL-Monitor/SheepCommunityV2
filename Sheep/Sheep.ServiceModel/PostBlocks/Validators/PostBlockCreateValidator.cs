using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.PostBlocks.Validators
{
    /// <summary>
    ///     新建一个帖子屏蔽的校验器。
    /// </summary>
    public class PostBlockCreateValidator : AbstractValidator<PostBlockCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="PostBlockCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostBlockCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                  });
        }
    }
}