﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Chapters.Validators
{
    /// <summary>
    ///     显示一条章注释的校验器。
    /// </summary>
    public class ChapterAnnotationShowValidator : AbstractValidator<ChapterAnnotationShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ChapterAnnotationShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ChapterAnnotationShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(Resources.ChapterNumberRequired);
                                     RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(Resources.AnnotationNumberRequired);
                                 });
        }
    }
}