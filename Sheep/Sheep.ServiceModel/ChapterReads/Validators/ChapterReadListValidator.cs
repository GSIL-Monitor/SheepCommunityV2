using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.ChapterReads.Validators
{
    /// <summary>
    ///     根据章列举一组阅读的校验器。
    /// </summary>
    public class ChapterReadListByChapterValidator : AbstractValidator<ChapterReadListByChapter>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "UserId",
                                                              "CreatedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ChapterReadListByChapterValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterReadListByChapterValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ChapterId).NotEmpty().WithMessage(x => string.Format(Resources.ChapterIdRequired));
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据用户列举一组阅读的校验器。
    /// </summary>
    public class ChapterReadListByUserValidator : AbstractValidator<ChapterReadListByUser>
    {
        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "ChapterId",
                                                              "CreatedDate"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="ChapterReadListByUserValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterReadListByUserValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}