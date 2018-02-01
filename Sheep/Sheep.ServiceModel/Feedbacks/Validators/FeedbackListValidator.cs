using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Feedbacks.Validators
{
    /// <summary>
    ///     查询并列举一组反馈的校验器。
    /// </summary>
    public class FeedbackListValidator : AbstractValidator<FeedbackList>
    {
        public static readonly HashSet<string> Statuses = new HashSet<string>
                                                          {
                                                              "待处理",
                                                              "提交技术",
                                                              "提交产品",
                                                              "提交运营",
                                                              "等待删除"
                                                          };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="FeedbackListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.Status).Must(status => Statuses.Contains(status)).WithMessage(x => string.Format(Resources.StatusRangeMismatch, Statuses.Join(","))).When(x => !x.Status.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据用户查询并列举一组反馈的校验器。
    /// </summary>
    public class FeedbackListByUserValidator : AbstractValidator<FeedbackListByUser>
    {
        public static readonly HashSet<string> Statuses = new HashSet<string>
                                                          {
                                                              "待处理",
                                                              "提交技术",
                                                              "提交产品",
                                                              "提交运营",
                                                              "等待删除"
                                                          };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="FeedbackListByUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackListByUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                     RuleFor(x => x.Status).Must(status => Statuses.Contains(status)).WithMessage(x => string.Format(Resources.StatusRangeMismatch, Statuses.Join(","))).When(x => !x.Status.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}