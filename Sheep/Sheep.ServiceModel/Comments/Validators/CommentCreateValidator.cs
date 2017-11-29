﻿using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Comments.Validators
{
    /// <summary>
    ///     创建一个评论的校验器。
    /// </summary>
    public class CommentCreateValidator : AbstractValidator<CommentCreate>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子"
                                                             };

        /// <summary>
        ///     初始化一个新的<see cref="CommentCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public CommentCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ParentType).NotEmpty().WithMessage(Resources.ParentTypeRequired);
                                      RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(Resources.ParentTypeRangeMismatch, ParentTypes.Join(",")).When(x => !x.ParentType.IsNullOrEmpty());
                                      RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                      RuleFor(x => x.Content).NotEmpty().WithMessage(Resources.ContentRequired);
                                  });
        }
    }
}