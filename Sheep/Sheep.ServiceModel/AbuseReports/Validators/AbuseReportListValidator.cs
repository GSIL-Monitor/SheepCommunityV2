﻿using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.AbuseReports.Validators
{
    /// <summary>
    ///     查询并列举一组举报的校验器。
    /// </summary>
    public class AbuseReportListValidator : AbstractValidator<AbuseReportList>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "用户",
                                                                 "帖子",
                                                                 "评论",
                                                                 "回复"
                                                             };

        public static readonly HashSet<string> Statuses = new HashSet<string>
                                                          {
                                                              "待处理",
                                                              "正常",
                                                              "删除内容",
                                                              "封禁用户",
                                                              "等待删除"
                                                          };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="AbuseReportListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AbuseReportListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentType).Must(parentType => ParentTypes.Contains(parentType)).WithMessage(x => string.Format(Resources.ParentTypeRangeMismatch, ParentTypes.Join(","))).When(x => !x.ParentType.IsNullOrEmpty());
                                     RuleFor(x => x.Status).Must(status => Statuses.Contains(status)).WithMessage(x => string.Format(Resources.StatusRangeMismatch, Statuses.Join(","))).When(x => !x.Status.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据上级查询并列举一组举报的校验器。
    /// </summary>
    public class AbuseReportListByParentValidator : AbstractValidator<AbuseReportListByParent>
    {
        public static readonly HashSet<string> Statuses = new HashSet<string>
                                                          {
                                                              "待处理",
                                                              "正常",
                                                              "删除内容",
                                                              "封禁用户",
                                                              "等待删除"
                                                          };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="AbuseReportListByParentValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AbuseReportListByParentValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ParentId).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdRequired));
                                     RuleFor(x => x.Status).Must(status => Statuses.Contains(status)).WithMessage(x => string.Format(Resources.StatusRangeMismatch, Statuses.Join(","))).When(x => !x.Status.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据用户查询并列举一组举报的校验器。
    /// </summary>
    public class AbuseReportListByUserValidator : AbstractValidator<AbuseReportListByUser>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "用户",
                                                                 "帖子",
                                                                 "评论",
                                                                 "回复"
                                                             };

        public static readonly HashSet<string> Statuses = new HashSet<string>
                                                          {
                                                              "待处理",
                                                              "正常",
                                                              "删除内容",
                                                              "封禁用户",
                                                              "等待删除"
                                                          };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="AbuseReportListByUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AbuseReportListByUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                     RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ParentTypeRangeMismatch, ParentTypes.Join(","))).When(x => !x.ParentType.IsNullOrEmpty());
                                     RuleFor(x => x.Status).Must(status => Statuses.Contains(status)).WithMessage(x => string.Format(Resources.StatusRangeMismatch, Statuses.Join(","))).When(x => !x.Status.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}