using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Posts.Validators
{
    /// <summary>
    ///     推送一个帖子的校验器。
    /// </summary>
    public class PostPushValidator : AbstractValidator<PostPush>
    {
        /// <summary>
        ///     初始化一个新的<see cref="PostPushValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostPushValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                 {
                                     RuleFor(x => x.PostId).NotEmpty().WithMessage(x => string.Format(Resources.PostIdRequired));
                                 });
        }
    }
}