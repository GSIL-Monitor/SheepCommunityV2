using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     更改显示名称的校验器。
    /// </summary>
    public class GroupChangeDisplayNameValidator : AbstractValidator<GroupChangeDisplayName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupChangeDisplayNameValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupChangeDisplayNameValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(Resources.DisplayNameRequired);
                                 });
        }
    }
}