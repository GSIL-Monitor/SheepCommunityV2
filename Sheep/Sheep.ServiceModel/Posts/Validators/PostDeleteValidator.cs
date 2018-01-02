using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Posts.Validators
{
    /// <summary>
    ///     删除一个帖子的校验器。
    /// </summary>
    public class PostDeleteValidator : AbstractValidator<PostDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="PostDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.PostId).NotEmpty().WithMessage(x => string.Format(Resources.PostIdRequired));
                                    });
        }
    }
}