using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Comments.Validators
{
    /// <summary>
    ///     更新一个评论的校验器。
    /// </summary>
    public class CommentUpdateValidator : AbstractValidator<CommentUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CommentUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.CommentId).NotEmpty().WithMessage(x => string.Format(Resources.CommentIdRequired));
                                     RuleFor(x => x.Content).NotEmpty().WithMessage(x => string.Format(Resources.ContentRequired));
                                 });
        }
    }
}