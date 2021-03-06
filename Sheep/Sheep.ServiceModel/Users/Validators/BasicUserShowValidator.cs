﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Users.Validators
{
    /// <summary>
    ///     显示一个用户基本信息的校验器。
    /// </summary>
    public class BasicUserShowValidator : AbstractValidator<BasicUserShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicUserShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicUserShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserId).NotEmpty().WithMessage(x => string.Format(Resources.UserIdRequired));
                                 });
        }
    }

    /// <summary>
    ///     根据用户名称或电子邮件地址显示一个用户基本信息的校验器。
    /// </summary>
    public class BasicUserShowByUserNameOrEmailValidator : AbstractValidator<BasicUserShowByUserNameOrEmail>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicUserShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicUserShowByUserNameOrEmailValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.UserNameOrEmail).NotEmpty().WithMessage(x => string.Format(Resources.UserNameOrEmailRequired));
                                 });
        }
    }

    /// <summary>
    ///     根据显示名称显示一个用户基本信息的校验器。
    /// </summary>
    public class BasicUserShowByDisplayNameValidator : AbstractValidator<BasicUserShowByDisplayName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicUserShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicUserShowByDisplayNameValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(x => string.Format(Resources.DisplayNameRequired));
                                 });
        }
    }
}