using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Comments.Validators
{
    /// <summary>
    ///     根据上级统计一组评论数量的校验器。
    /// </summary>
    public class CommentCountByParentValidator : AbstractValidator<CommentCountByParent>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CommentCountByParentValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentCountByParentValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdRequired));
                                 });
        }
    }

    /// <summary>
    ///     根据上级列表统计一组评论数量的校验器。
    /// </summary>
    public class CommentCountByParentsValidator : AbstractValidator<CommentCountByParents>
    {
        /// <summary>
        ///     初始化一个新的<see cref="CommentCountByParentValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentCountByParentsValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentIds).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdsRequired));
                                 });
        }
    }
}