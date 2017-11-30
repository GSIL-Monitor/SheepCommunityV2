using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Replies.Validators
{
    /// <summary>
    ///     根据上级查询并列举一组回复的校验器。
    /// </summary>
    public class ReplyListByParentValidator : AbstractValidator<ReplyListByParent>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "VotesCount",
                                                              "YesVotesCount",
                                                              "NoVotesCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ReplyListByParentValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ReplyListByParentValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据用户查询并列举一组回复的校验器。
    /// </summary>
    public class ReplyListByUserValidator : AbstractValidator<ReplyListByUser>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "VotesCount",
                                                              "YesVotesCount",
                                                              "NoVotesCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ReplyListByUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ReplyListByUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}