using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.PostBlocks.Validators
{
    /// <summary>
    ///     查询并列举一组帖子屏蔽的校验器。
    /// </summary>
    public class PostBlockListValidator : AbstractValidator<PostBlockList>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="PostBlockListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostBlockListValidator()
        {
            RuleSet(ApplyTo.Get, () => { RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty()); });
        }
    }

    /// <summary>
    ///     根据上级查询并列举一组帖子屏蔽的校验器。
    /// </summary>
    public class PostBlockListByPostValidator : AbstractValidator<PostBlockListByPost>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="PostBlockListByPostValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostBlockListByPostValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.PostId).NotEmpty().WithMessage(x => string.Format(Resources.PostIdRequired));
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据屏蔽者查询并列举一组帖子屏蔽的校验器。
    /// </summary>
    public class PostBlockListByBlockerValidator : AbstractValidator<PostBlockListByBlocker>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "CreatedDate",
                                                              "ModifiedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="PostBlockListByBlockerValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostBlockListByBlockerValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}