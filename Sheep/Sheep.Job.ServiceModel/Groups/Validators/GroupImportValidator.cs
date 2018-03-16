using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Job.ServiceModel.Properties;

namespace Sheep.Job.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     查询并导入一组群组的校验器。
    /// </summary>
    public class GroupImportValidator : AbstractValidator<GroupImport>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="GroupImportValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupImportValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}