using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     查询并列举一组卷注释的校验器。
    /// </summary>
    public class VolumeAnnotationListValidator : AbstractValidator<VolumeAnnotationList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Number"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="VolumeAnnotationListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeAnnotationListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}