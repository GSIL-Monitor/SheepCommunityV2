﻿using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     查询并列举一组卷的校验器。
    /// </summary>
    public class VolumeListValidator : AbstractValidator<VolumeList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Number",
                                                              "ChaptersCount",
                                                              "SubjectsCount"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="VolumeListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(Resources.OrderByRangeMismatch, OrderBys.Join(",")).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}