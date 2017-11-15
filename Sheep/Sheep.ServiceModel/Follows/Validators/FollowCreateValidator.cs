﻿using System;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Follows.Validators
{
    /// <summary>
    ///     创建关注的校验器。
    /// </summary>
    public class FollowCreateValidator : AbstractValidator<FollowCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="FollowCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public FollowCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.FollowingUserId).Must(UserIdExists).WithMessage(Resources.FollowingUserIdNotExists);
                                  });
        }

        private bool UserIdExists(int userId)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(Request);
            using (authRepo as IDisposable)
            {
                return ((IUserAuthRepository) authRepo).GetUserAuth(userId.ToString()) != null;
            }
        }
    }
}