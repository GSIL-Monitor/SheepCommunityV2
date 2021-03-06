﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     显示一个用户的校验器。
    /// </summary>
    public class UserShowValidator : AbstractValidator<UserShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="UserShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                 });
        }
    }

    /// <summary>
    ///     根据用户名称或电子邮件地址显示一个用户的校验器。
    /// </summary>
    public class UserShowByUserNameOrEmailValidator : AbstractValidator<UserShowByUserNameOrEmail>
    {
        /// <summary>
        ///     初始化一个新的<see cref="UserShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserShowByUserNameOrEmailValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserNameOrEmail).NotEmpty().WithMessage(x => string.Format(Resources.UserNameOrEmailRequired));
                                 });
        }
    }

    /// <summary>
    ///     根据显示名称显示一个用户的校验器。
    /// </summary>
    public class UserShowByDisplayNameValidator : AbstractValidator<UserShowByDisplayName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="UserShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserShowByDisplayNameValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(x => string.Format(Resources.DisplayNameRequired));
                                 });
        }
    }
}