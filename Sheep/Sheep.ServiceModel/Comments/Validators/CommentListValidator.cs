using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Comments.Validators
{
    /// <summary>
    ///     根据上级查询并列举一组评论的校验器。
    /// </summary>
    public class CommentListByParentValidator : AbstractValidator<CommentListByParent>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "RepliesCount",
                                                              "VotesCount",
                                                              "YesVotesCount",
                                                              "NoVotesCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="CommentListByParentValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentListByParentValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdRequired));
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据用户查询并列举一组评论的校验器。
    /// </summary>
    public class CommentListByUserValidator : AbstractValidator<CommentListByUser>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子",
                                                                 "章",
                                                                 "节"
                                                             };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "RepliesCount",
                                                              "VotesCount",
                                                              "YesVotesCount",
                                                              "NoVotesCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="CommentListByUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentListByUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                     RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ParentTypeRangeMismatch, ParentTypes.Join(","))).When(x => !x.ParentType.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}