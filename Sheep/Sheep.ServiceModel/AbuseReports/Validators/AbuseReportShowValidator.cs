using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.AbuseReports.Validators
{
    /// <summary>
    ///     显示一个举报的校验器。
    /// </summary>
    public class AbuseReportShowValidator : AbstractValidator<AbuseReportShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AbuseReportShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AbuseReportShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ReportId).NotEmpty().WithMessage(x => string.Format(Resources.ReportIdRequired));
                                 });
        }
    }
}