using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Comments.Validators
{
    /// <summary>
    ///     显示一个评论的校验器。
    /// </summary>
    public class CommentShowValidator : AbstractValidator<CommentShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CommentShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.CommentId).NotEmpty().WithMessage(Resources.CommentIdRequired);
                                 });
        }
    }
}