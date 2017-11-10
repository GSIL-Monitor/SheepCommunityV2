using System;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改头像的校验器。
    /// </summary>
    public class AccountChangeAvatarValidator : AbstractValidator<AccountChangeAvatar>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeAvatarValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeAvatarValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.SourceAvatarUrl).Must(url => url.GetImageUrlExtension().IsImageExtension()).WithMessage(Resources.SourceAvatarUrlMismatch).When(x => !x.SourceAvatarUrl.IsNullOrEmpty());
                                 });
        }
    }
}