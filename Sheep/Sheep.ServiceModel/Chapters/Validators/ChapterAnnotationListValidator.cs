using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     查询并列举一组章注释的校验器。
    /// </summary>
    public class ChapterAnnotationListValidator : AbstractValidator<ChapterAnnotationList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Number"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ChapterAnnotationListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterAnnotationListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}