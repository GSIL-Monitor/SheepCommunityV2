﻿using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Bookmarks.Validators
{
    /// <summary>
    ///     新建一个收藏的校验器。
    /// </summary>
    public class BookmarkCreateValidator : AbstractValidator<BookmarkCreate>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子",
                                                                 "章",
                                                                 "节"
                                                             };

        /// <summary>
        ///     初始化一个新的<see cref="BookmarkCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookmarkCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ParentType).NotEmpty().WithMessage(x => string.Format(Resources.ParentTypeRequired));
                                      RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ParentTypeRangeMismatch, ParentTypes.Join(","))).When(x => !x.ParentType.IsNullOrEmpty());
                                      RuleFor(x => x.ParentId).NotEmpty().WithMessage(x => string.Format(Resources.ParentIdRequired));
                                  });
        }
    }
}