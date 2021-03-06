﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Subjects.Validators
{
    /// <summary>
    ///     更新一个主题的校验器。
    /// </summary>
    public class SubjectUpdateValidator : AbstractValidator<SubjectUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="SubjectUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public SubjectUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                     RuleFor(x => x.SubjectNumber).NotEmpty().WithMessage(x => string.Format(Resources.SubjectNumberRequired));
                                     RuleFor(x => x.Title).NotEmpty().WithMessage(x => string.Format(Resources.TitleRequired));
                                 });
        }
    }
}