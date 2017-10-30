using System;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.Common.Auth;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改显示名称的校验器。
    /// </summary>
    public class AccountChangeDisplayNameValidator : AbstractValidator<AccountChangeDisplayName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeDisplayNameValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeDisplayNameValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(Resources.DisplayNameRequired);
                                 });
        }
    }
}