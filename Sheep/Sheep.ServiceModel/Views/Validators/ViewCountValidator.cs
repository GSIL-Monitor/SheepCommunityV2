﻿using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     根据上级统计一组阅读数量的校验器。
    /// </summary>
    public class ViewCountByParentValidator : AbstractValidator<ViewCountByParent>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ViewCountByParentValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewCountByParentValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                 });
        }
    }

    /// <summary>
    ///     根据用户统计一组阅读数量的校验器。
    /// </summary>
    public class ViewCountByUserValidator : AbstractValidator<ViewCountByUser>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子",
                                                                 "章",
                                                                 "节"
                                                             };

        /// <summary>
        ///     初始化一个新的<see cref="ViewCountByUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewCountByUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(Resources.UserIdRequired);
                                     RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(Resources.ParentTypeRangeMismatch, ParentTypes.Join(",")).When(x => !x.ParentType.IsNullOrEmpty());
                                 });
        }
    }
}