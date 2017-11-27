using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Likes.Validators
{
    /// <summary>
    ///     列举一组用户的校验器。
    /// </summary>
    public class LikeListValidator : AbstractValidator<LikeList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="LikeListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public LikeListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}