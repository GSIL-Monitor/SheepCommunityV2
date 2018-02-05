using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.AbuseReports.Validators
{
    /// <summary>
    ///     创建一个举报的校验器。
    /// </summary>
    public class AbuseReportCreateValidator : AbstractValidator<AbuseReportCreate>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "用户",
                                                                 "帖子",
                                                                 "评论",
                                                                 "回复"
                                                             };

        /// <summary>
        ///     初始化一个新的<see cref="AbuseReportCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AbuseReportCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ParentType).NotEmpty().WithMessage(x => string.Format(Resources.ParentTypeRequired));
                                      RuleFor(x => x.ParentType).Must(parentType => ParentTypes.Contains(parentType)).WithMessage(x => string.Format(Resources.ParentTypeRangeMismatch, ParentTypes.Join(","))).When(x => !x.ParentType.IsNullOrEmpty());
                                      RuleFor(x => x.ParentId).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdRequired));
                                      RuleFor(x => x.Reason).NotEmpty().WithMessage(x => string.Format(Resources.ReasonRequired));
                                  });
        }
    }
}