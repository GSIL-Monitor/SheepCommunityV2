using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Comments.Validators
{
    /// <summary>
    ///     删除一个评论的校验器。
    /// </summary>
    public class CommentDeleteValidator : AbstractValidator<CommentDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CommentDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.CommentId).NotEmpty().WithMessage(x => string.Format(Resources.CommentIdRequired));
                                    });
        }
    }
}