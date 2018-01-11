using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Job.ServiceModel.Properties;

namespace Sheep.Job.ServiceModel.Replies.Validators
{
    /// <summary>
    ///     查询并计算一组回复分数的校验器。
    /// </summary>
    public class ReplyCalculateValidator : AbstractValidator<ReplyCalculate>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "评论"
                                                             };

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
        ///     初始化一个新的<see cref="ReplyCalculateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ReplyCalculateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ParentTypeRangeMismatch, ParentTypes.Join(","))).When(x => !x.ParentType.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}