using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     创建群组的校验器。
    /// </summary>
    public class GroupCreateValidator : AbstractValidator<GroupCreate>
    {
        public static readonly HashSet<string> JoinModes = new HashSet<string>
                                                           {
                                                               "Direct",
                                                               "RequireVerification",
                                                               "Joinless"
                                                           };

        /// <summary>
        ///     初始化一个新的<see cref="GroupCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.DisplayName).NotEmpty().WithMessage(Resources.DisplayNameRequired);
                                      RuleFor(x => x.JoinMode).Must(joinMode => JoinModes.Contains(joinMode)).WithMessage(Resources.JoinModeRangeMismatch, JoinModes.Join(",")).When(x => !x.JoinMode.IsNullOrEmpty());
                                  });
        }
    }
}