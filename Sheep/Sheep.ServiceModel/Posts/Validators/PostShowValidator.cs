﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Posts.Validators
{
    /// <summary>
    ///     显示一个帖子的校验器。
    /// </summary>
    public class PostShowValidator : AbstractValidator<PostShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="PostShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public PostShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.PostId).NotEmpty().WithMessage(x => string.Format(Resources.PostIdRequired));
                                 });
        }
    }
}