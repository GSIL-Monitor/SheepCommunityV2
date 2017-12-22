using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     根据上级列举一组查看的校验器。
    /// </summary>
    public class ViewListByParentValidator : AbstractValidator<ViewListByParent>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ViewListByParentValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewListByParentValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据用户列举一组查看的校验器。
    /// </summary>
    public class ViewListByUserValidator : AbstractValidator<ViewListByUser>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子",
                                                                 "章",
                                                                 "节"
                                                             };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ViewListByUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewListByUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                     RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(Resources.ParentTypeRangeMismatch, ParentTypes.Join(",")).When(x => !x.ParentType.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}