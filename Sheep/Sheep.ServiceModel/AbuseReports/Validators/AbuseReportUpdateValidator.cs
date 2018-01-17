using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.AbuseReports.Validators
{
    /// <summary>
    ///     更新一个举报的校验器。
    /// </summary>
    public class AbuseReportUpdateValidator : AbstractValidator<AbuseReportUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AbuseReportUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AbuseReportUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.ReportId).NotEmpty().WithMessage(x => string.Format(Resources.ReportIdRequired));
                                     RuleFor(x => x.Reason).NotEmpty().WithMessage(x => string.Format(Resources.ReasonRequired));
                                 });
        }
    }
}