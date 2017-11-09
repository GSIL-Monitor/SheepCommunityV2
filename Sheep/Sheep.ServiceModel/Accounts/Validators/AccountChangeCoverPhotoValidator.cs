using System;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改封面图片的校验器。
    /// </summary>
    public class AccountChangeCoverPhotoValidator : AbstractValidator<AccountChangeCoverPhoto>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeCoverPhotoValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeCoverPhotoValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.SourceUrl).Must(url => url.GetImageUrlExtension().IsImageExtension()).WithMessage(Resources.SourceUrlMismatch).When(x => !x.SourceUrl.IsNullOrEmpty());
                                 });
        }
    }
}