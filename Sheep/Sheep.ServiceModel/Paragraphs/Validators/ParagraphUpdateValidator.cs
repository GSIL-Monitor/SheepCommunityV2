﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Paragraphs.Validators
{
    /// <summary>
    ///     更新一节的校验器。
    /// </summary>
    public class ParagraphUpdateValidator : AbstractValidator<ParagraphUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="ParagraphUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ParagraphUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                     RuleFor(x => x.ChapterNumber).NotEmpty().WithMessage(x => string.Format(Resources.ChapterNumberRequired));
                                     RuleFor(x => x.ParagraphNumber).NotEmpty().WithMessage(x => string.Format(Resources.ParagraphNumberRequired));
                                     RuleFor(x => x.Content).NotEmpty().WithMessage(x => string.Format(Resources.ContentRequired));
                                 });
        }
    }
}