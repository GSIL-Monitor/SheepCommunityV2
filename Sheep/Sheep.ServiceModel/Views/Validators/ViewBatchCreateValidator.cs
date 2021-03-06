﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     新建一组节查看的校验器。
    /// </summary>
    public class ViewBatchCreateForParagraphsValidator : AbstractValidator<ViewBatchCreateForParagraphs>
    {
        /// <summary>
        ///     初始化一组新的<see cref="ViewBatchCreateForParagraphsValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewBatchCreateForParagraphsValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                      RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                      RuleFor(x => x.BeginChapterNumber).NotEmpty().WithMessage(x => string.Format(Resources.BeginChapterNumberRequired));
                                      RuleFor(x => x.BeginParagraphNumber).NotEmpty().WithMessage(x => string.Format(Resources.BeginParagraphNumberRequired));
                                      RuleFor(x => x.EndChapterNumber).NotEmpty().WithMessage(x => string.Format(Resources.EndChapterNumberRequired));
                                      RuleFor(x => x.EndParagraphNumber).NotEmpty().WithMessage(x => string.Format(Resources.EndParagraphNumberRequired));
                                  });
        }
    }
}