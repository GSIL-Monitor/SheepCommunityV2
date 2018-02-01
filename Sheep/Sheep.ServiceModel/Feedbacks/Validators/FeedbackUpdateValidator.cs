using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Feedbacks.Validators
{
    /// <summary>
    ///     更新一个反馈的校验器。
    /// </summary>
    public class FeedbackUpdateValidator : AbstractValidator<FeedbackUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FeedbackUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.FeedbackId).NotEmpty().WithMessage(x => string.Format(Resources.FeedbackIdRequired));
                                     RuleFor(x => x.Content).NotEmpty().WithMessage(x => string.Format(Resources.ContentRequired));
                                 });
        }
    }

    /// <summary>
    ///     更新一个反馈的校验器。
    /// </summary>
    public class FeedbackUpdateStatusValidator : AbstractValidator<FeedbackUpdateStatus>
    {
        public static readonly HashSet<string> Statuses = new HashSet<string>
                                                          {
                                                              "待处理",
                                                              "提交技术",
                                                              "提交产品",
                                                              "提交运营",
                                                              "等待删除"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="FeedbackUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FeedbackUpdateStatusValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.FeedbackId).NotEmpty().WithMessage(x => string.Format(Resources.FeedbackIdRequired));
                                     RuleFor(x => x.Status).Must(status => Statuses.Contains(status)).WithMessage(x => string.Format(Resources.StatusRangeMismatch, Statuses.Join(","))).When(x => !x.Status.IsNullOrEmpty());
                                 });
        }
    }
}