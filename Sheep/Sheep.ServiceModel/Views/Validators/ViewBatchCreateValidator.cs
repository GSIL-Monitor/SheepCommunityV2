using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     新建一组节阅读的校验器。
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
                                      RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                      RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                      RuleFor(x => x.BeginChapterNumber).NotEmpty().WithMessage(Resources.BeginChapterNumberRequired);
                                      RuleFor(x => x.BeginParagraphNumber).NotEmpty().WithMessage(Resources.BeginParagraphNumberRequired);
                                      RuleFor(x => x.EndChapterNumber).NotEmpty().WithMessage(Resources.EndChapterNumberRequired);
                                      RuleFor(x => x.EndParagraphNumber).NotEmpty().WithMessage(Resources.EndParagraphNumberRequired);
                                  });
        }
    }
}