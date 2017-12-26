using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Bookmarks.Validators
{
    /// <summary>
    ///     新建一组收藏的校验器。
    /// </summary>
    public class BookmarkBatchCreateForParagraphsValidator : AbstractValidator<BookmarkBatchCreateForParagraphs>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BookmarkBatchCreateForParagraphsValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BookmarkBatchCreateForParagraphsValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ParagraphIds).NotNull().WithMessage(Resources.ParagraphIdsRequired);
                                      RuleFor(x => x.ParagraphIds).Must(list => !list.Exists(id => id.IsNullOrEmpty())).WithMessage(Resources.ParagraphIdsValuesMismatch);
                                  });
        }
    }
}