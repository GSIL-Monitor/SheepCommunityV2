using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.AbuseReports.Validators
{
    /// <summary>
    ///     删除一个举报的校验器。
    /// </summary>
    public class AbuseReportDeleteValidator : AbstractValidator<AbuseReportDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AbuseReportDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AbuseReportDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.ReportId).NotEmpty().WithMessage(x => string.Format(Resources.ReportIdRequired));
                                    });
        }
    }
}